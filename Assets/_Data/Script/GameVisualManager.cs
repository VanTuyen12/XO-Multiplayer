using System;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameVisualManager : MyNetWorkMonoBehaviour
{
    [SerializeField] private Transform _crossPrefab;
    [SerializeField] private Transform _circlePrefab;
    private Transform poolHolder;

    protected override void Start()
    {
        base.Start();   
        GameEvent.OnClickGridPosition += OnClickOnGridPosition;
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
        
        Debug.Log(transform.name+ " :LoadPoolHolder", gameObject);
    }
    
    private void OnClickOnGridPosition(object sender, GameEvent.OnClickGridPositionEventArgs e)
    {
        Debug.Log("OnClickOnGridPosition");
        GridPosition gridpos = sender as GridPosition;
        if (gridpos == null) return;
        
        Vector3 gridPos = gridpos.transform.position;
        Vector3 gridScale = gridpos.transform.localScale;
       
        SpawnPrefabsRpc(gridPos, gridScale, e.playerType);
    }
    
    [Rpc(SendTo.Server)]
    protected virtual void SpawnPrefabsRpc(Vector3 gridPos, Vector3 gridScale,EnumPlayerType playerType )
    {
        Debug.Log("SpawnObject");
        Transform prefab = SelectPrefab(playerType);
        
        var newPrefab = Instantiate(prefab);
        newPrefab.transform.localPosition = gridPos;
        newPrefab.transform.localScale = gridScale;
        
        newPrefab.GetComponent<NetworkObject>().Spawn(true);
        newPrefab.GetComponent<NetworkObject>().TrySetParent(poolHolder, false);
    }

    protected virtual Transform SelectPrefab(EnumPlayerType playerType)
    {
        switch (playerType)
        {
            case EnumPlayerType.None:
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
    }
}