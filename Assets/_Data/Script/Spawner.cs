using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Spawner<T> : MyMonoBehaviour where T : PoolObj
{
    private int spawnCount = 0;
    
    [SerializeField]protected PoolHolder poolHolder;

    [SerializeField] protected PoolPrefabs<T> poolPrefabs;
    public PoolPrefabs<T> PoolPrefabs => poolPrefabs;
    
    [SerializeField]protected List<T> inPoolObjs = new List<T>();
    
    public virtual T Spawn(T prefab, Vector3 position)
    {
        T newPrefab = Spawn(prefab);
        newPrefab.transform.position = position;
        
        return newPrefab;
    }

    public virtual T Spawn(T prefab)
    {
        T newPrefab = GetObjFromPool(prefab);
        if (newPrefab == null)
        {
            newPrefab = Instantiate(prefab);
            spawnCount++;
            UpdateNamePrefabs(newPrefab,prefab);
        }

        if (poolHolder != null) newPrefab.transform.SetParent(poolHolder.transform); 
        
        return newPrefab;
    }

    protected virtual void UpdateNamePrefabs(T newPrefab, T prefab)
    {
        newPrefab.name = prefab.name + " _ "+ spawnCount;   
    }

    protected virtual T GetObjFromPool(T prefab)
    {
        foreach (var poolObj in inPoolObjs)
        {
            if (poolObj.GetName() == prefab.GetName())
            {
                RemoveObjectFromPool(poolObj);
                return poolObj;
            }
        }
        return null;
    }

    public void Despawn(T objToDespawn)
    {
        if (objToDespawn is MonoBehaviour monoBehaviour)
        {
            monoBehaviour.gameObject.SetActive(false);
            AddObjectToPool(objToDespawn);
        }
    }
    
    public virtual void RemoveObjectFromPool(T prefab)
    {
        this.inPoolObjs.Remove(prefab);
    }

    public virtual void AddObjectToPool(T prefab)
    {
        this.inPoolObjs.Add(prefab);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPoolHolder();
        this.LoadPoolPrefabs();
    }

    protected virtual void LoadPoolPrefabs()
    {
        if(poolPrefabs != null) return;
        poolPrefabs = GetComponentInChildren<PoolPrefabs<T>>();
        
        Debug.Log(transform.name + ":LoadPoolHolder",gameObject);
    }

    protected virtual void LoadPoolHolder()
    {
        if(poolHolder != null) return;
        poolHolder = GetComponentInChildren<PoolHolder>()?? new GameObject("PoolHolder").AddComponent<PoolHolder>();

        if (poolHolder.transform.parent != this.transform)
            poolHolder.transform.SetParent(this.transform);
        
        if (!poolHolder.GetComponent<NetworkObject>())
           poolHolder.gameObject.AddComponent<NetworkObject>();
        
        var poolHolderNetworkObj = poolHolder.GetComponent<NetworkObject>();
        if (!poolHolderNetworkObj.IsSpawned)
            poolHolderNetworkObj.Spawn(true);
       
        Debug.Log(transform.name + ":LoadPoolHolder",gameObject);
    }
}

