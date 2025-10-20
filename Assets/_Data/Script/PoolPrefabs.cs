using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PoolPrefabs<T> : MyNetWorkMonoBehaviour where T : NetworkBehaviour
{
    
    [SerializeField] protected string resourcePath = "Prefabs";
    [SerializeField] protected List<T> poolPrefabs = new();
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPoolPrefabs();
        
    }

    protected virtual void LoadPoolPrefabs()
    {
        if (poolPrefabs.Count > 0) return;
        T[] prefabsInRources = Resources.LoadAll<T>(resourcePath);

        if (prefabsInRources.Length <= 0)
        {
            foreach (Transform child in transform)
            {
                var prefabs = child.GetComponent<T>();
                if(prefabs!= null) poolPrefabs.Add(prefabs);
            }
        }
        else
        {
            foreach (T prefab in prefabsInRources)
            {
                poolPrefabs.Add(prefab);
            } 
        }
        
        Debug.Log($"Loaded {poolPrefabs.Count} prefabs of type {typeof(T).Name}");
        
    }
    
   
    protected virtual void HidePrefabs()
    {
        foreach (T prefab in poolPrefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }
    public virtual T GetByName(string prefabName)
    {
        foreach (T prefab in poolPrefabs)
        {
            if (prefab.name == prefabName )
            {
                return prefab;
            }
        }
        return null;
    }
}
