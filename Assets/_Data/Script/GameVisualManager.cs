using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GameVisualManager : MyNetWorkMonoBehaviour
{
    
    [SerializeField] private List<GameObject> _visualGameObjectList = new();
    [SerializeField] protected GameObjectSpawner gameObjectSpawner;
    protected override void Start()
    {
        base.Start();
        GameEvent.OnClickGridPosition += OnClickOnGridPosition;
        GameEvent.OnWinGame += GameEventOnWinGame;
        GameEvent.OnRematch += GameEventOnRematch;
    }

    private void GameEventOnRematch(object sender, EventArgs e)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        foreach (var obj in _visualGameObjectList)
        {
            Destroy(obj);
        }
        _visualGameObjectList.Clear();
    }

    private void GameEventOnWinGame(object obj, WinResult winResult)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        
        if (GridManager.Instance == null) return;
        var startPos = new Vector2Int(winResult.posStart.x, winResult.posStart.y);
        var endPos = new Vector2Int(winResult.posEnd.x, winResult.posEnd.y);
        Transform cellStartPos = GridManager.Instance.FindCell(startPos.x, startPos.y);
        Transform cellEndPos = GridManager.Instance.FindCell(endPos.x, endPos.y);
       
        var dir = cellEndPos.transform.position - cellStartPos.transform.position;

        var centerPos = (startPos + endPos) / 2;
        Transform cellTargetPos = GridManager.Instance.FindCell(centerPos.x, centerPos.y);

        float scaleRatio = GridManager.Instance.ScaleRatio;
        float angle = AngleLine(winResult.winDirection);
        Vector3 lineLength = new Vector3(dir.magnitude * 0.8f, scaleRatio * 0.6f, 1);
        
        SpawnWinLine(cellTargetPos.transform.position,angle,lineLength);
    }

    protected virtual void SpawnWinLine(Vector3 winLinePos,float angle,Vector3 lineLength)
    {
        var winLine =gameObjectSpawner.PoolPrefabs.GetByName(EnumPlayerType.WinLine.ToString());
        if (winLine == null) Debug.Log(transform.name+" :NULL winLine");
        var newWinLine =  gameObjectSpawner.Spawn(winLine);
        if (newWinLine == null) Debug.Log(transform.name+" :NULL newWinLine");
        newWinLine.SetActive(true);
        
        newWinLine.transform.localPosition = winLinePos ;
        newWinLine.transform.localRotation = Quaternion.Euler(0, 0, angle);
        newWinLine.transform.localScale = lineLength;
        
        newWinLine.GetComponent<NetworkObject>().Spawn(true);
        _visualGameObjectList.Add(newWinLine.gameObject);
    }

    private int AngleLine(EnumDirection dir)
    {
        switch (dir)
        {
            case EnumDirection.Horizontal:
                return 90;
            case EnumDirection.DiagonalDown:
                return -45;
            case EnumDirection.DiagonalUp:
                return 45;
        }
        return 0;
    }


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameObjectSpawner();
    }

    protected virtual void LoadGameObjectSpawner()
    {
        if (gameObjectSpawner != null) return;
        gameObjectSpawner = GetComponent<GameObjectSpawner>();
        Debug.Log(transform.name + " :LoadGameObjectSpawner", gameObject);
    }

    private void OnClickOnGridPosition(object sender, GameEvent.OnClickGridPositionEventArgs e)
    {
        //Debug.Log("OnClickOnGridPosition");
        Vector3 gridPos = e.vtPos;
        Vector3 gridScale = e.vtScale;

        SpawnPrefabsRpc(gridPos, gridScale, e.playerType);
    }

    [Rpc(SendTo.Server)]
    protected virtual void SpawnPrefabsRpc(Vector3 gridPos, Vector3 gridScale, EnumPlayerType playerType)
    {
        //Debug.Log("SpawnObject");
        var prefab = SelectPrefab(playerType);
        if (prefab == null) Debug.Log(transform.name+" :NULL prefab");
        
        var newPrefab = gameObjectSpawner.Spawn(prefab);
        if (newPrefab == null) Debug.Log(transform.name+" :NULL newPrefab");
        
        newPrefab.SetActive(true);
        newPrefab.transform.localPosition = gridPos;
        newPrefab.transform.localScale = gridScale;

        var netObj = newPrefab.GetComponent<NetworkObject>();
        netObj.Spawn(true);
        
        _visualGameObjectList.Add(newPrefab.gameObject);
    }
    
    /*protected virtual void SpawnPrefab(EnumPlayerType playerType)
    {
        var prefab = SelectPrefab(playerType);
        switch (playerType)
        {
            default:
            case EnumPlayerType.Cross:
                    return (CrossCtrl)prefab;
            case EnumPlayerType.Circle:
                    return (CircleCtrl)prefab;
        }
       
    }*/
    protected virtual ObjectPrefabsCtrl SelectPrefab(EnumPlayerType playerType)
    {
       return gameObjectSpawner.PoolPrefabs.GetByName(playerType.ToString());
       
    }
    public override void OnDestroy()
    {
        GameEvent.OnClickGridPosition -= OnClickOnGridPosition;
        GameEvent.OnWinGame -= GameEventOnWinGame;
        GameEvent.OnRematch -= GameEventOnRematch;
    }
    
}