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
    [SerializeField] private Transform _crossPrefab;
    [SerializeField] private Transform _circlePrefab;
    [SerializeField] private Transform _winLine;
    [SerializeField] private Transform poolHolder;
    [SerializeField] private List<GameObject> _visualGameObjectList = new();
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
        var newWinLine = Instantiate(_winLine);
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
        this.LoadPooHolder();
    }

    protected virtual void LoadPooHolder()
    {
        if (poolHolder != null) return;
        poolHolder = transform.Find("PoolHolder") ??
                     new GameObject("PoolHolder").transform;

        if (poolHolder.transform.parent != this.transform)
            poolHolder.transform.SetParent(this.transform);

        if (!poolHolder.GetComponent<NetworkObject>())
            poolHolder.AddComponent<NetworkObject>();

        Debug.Log(transform.name + " :LoadPoolHolder", gameObject);
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
        Transform prefab = SelectPrefab(playerType);

        var newPrefab = Instantiate(prefab);
        newPrefab.transform.localPosition = gridPos;
        newPrefab.transform.localScale = gridScale;

        var netObj = newPrefab.GetComponent<NetworkObject>();
        netObj.Spawn(true);

        if (netObj.TrySetParent(poolHolder, false))
            newPrefab.transform.SetParent(poolHolder, false);
        
        _visualGameObjectList.Add(newPrefab.gameObject);
    }
    
    protected virtual Transform SelectPrefab(EnumPlayerType playerType)
    {
        switch (playerType)
        {
            default:
            case EnumPlayerType.Cross:
                return _crossPrefab;
            case EnumPlayerType.Circle:
                return _circlePrefab;
        }
    }

    private void OnDisable()
    {
        GameEvent.OnClickGridPosition -= OnClickOnGridPosition;
        GameEvent.OnWinGame -= GameEventOnWinGame;
        GameEvent.OnRematch -= GameEventOnRematch;
    }
    
}