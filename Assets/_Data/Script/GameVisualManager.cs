using System;
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

    protected override void Start()
    {
        base.Start();   
        GameEvent.OnClickGridPosition += OnClickOnGridPosition;
        GameEvent.OnWinGame += GameEventOnWinGame;
        
    }

    private void GameEventOnWinGame(object obj, WinResult winResult)
    {
        Debug.Log("========== WIN GAME OVER ===========");
        //Debug.Log(winResult.posStart + " " + winResult.posEnd);


        var startPos = new Vector2Int(winResult.posStart.x, winResult.posStart.y);
        var cellStartPos = GridManager.Instance.FindCell(startPos.x,startPos.y);
        var endPos = new Vector2Int(winResult.posEnd.x, winResult.posEnd.y);
        var cellEndPos = GridManager.Instance.FindCell(endPos.x,endPos.y);
        Debug.Log(startPos +" "+endPos);
        var dir = cellEndPos.transform.position - cellStartPos.transform.position;
        Debug.Log(dir.ToString());
        var centerPos = (startPos + endPos)/2;
        Debug.Log(centerPos.ToString());
        var cellTargetPos = GridManager.Instance.FindCell(centerPos.x,centerPos.y);
        Debug.Log(cellTargetPos.ToString());
        
        var newWinLine = Instantiate(_winLine);
        newWinLine.transform.localPosition = cellTargetPos.transform.position;
        newWinLine.transform.localScale = new Vector3(dir.magnitude, GridManager.Instance.ScaleRatio, 1);

    }


    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPooHolder();
    }

    protected virtual void LoadPooHolder()
    {
        if(poolHolder != null) return;
        poolHolder = transform.Find("PoolHolder") ?? 
                      new GameObject("PoolHolder").transform;
        
        if (poolHolder.transform.parent != this.transform)
            poolHolder.transform.SetParent(this.transform);
        
        if (!poolHolder.GetComponent<NetworkObject>())
            poolHolder.AddComponent<NetworkObject>();
        
        Debug.Log(transform.name+ " :LoadPoolHolder", gameObject);
    }
    
    private void OnClickOnGridPosition(object sender, GameEvent.OnClickGridPositionEventArgs e)
    {
        //Debug.Log("OnClickOnGridPosition");
        
        Vector3 gridPos = e.vtPos;
        Vector3 gridScale = e.vtScale;
       
        SpawnPrefabsRpc(gridPos, gridScale, e.playerType);
    }
    
    [Rpc(SendTo.Server)]
    protected virtual void SpawnPrefabsRpc(Vector3 gridPos, Vector3 gridScale,EnumPlayerType playerType )
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
        
    }
  
    protected virtual Transform SelectPrefab(EnumPlayerType playerType)
    {
        switch (playerType)
        {
            default:
                case EnumPlayerType.Cross :
                    return _crossPrefab;
                case EnumPlayerType.Circle:
                return _circlePrefab;
        }
    }
    private void OnDisable()
    {
        GameEvent.OnClickGridPosition -= OnClickOnGridPosition;
        GameEvent.OnWinGame -= GameEventOnWinGame;
    }
}