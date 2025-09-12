using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class GameVisualManager : MyMonoBehaviour
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

        GridPosition gridpos = sender as GridPosition;
        if (gridpos != null)
        {
            Vector3 gridSize = gridpos.transform.position;
            Vector3 gridScale = gridpos.transform.localScale;

            var newCross = SpawnCross(gridSize, gridScale);

            newCross.transform.SetParent(poolHolder.transform);
        }

    }

    protected virtual Transform SpawnCross(Vector3 pos, Vector3 scale)
    {
        var newPrefab = Instantiate(_crossPrefab, pos, Quaternion.identity);
        newPrefab.localScale = scale;

        return newPrefab;
    }
    
    
}