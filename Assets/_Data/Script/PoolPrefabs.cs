using System.Collections.Generic;
using UnityEngine;

public abstract class PoolPrefabs<T> : MyMonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected List<T> poolPrefabs = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPoolPrefabs();
        this.HidePrefabs();
    }

    protected virtual void LoadPoolPrefabs()
    {
        if (poolPrefabs.Count > 0) return;
        
        foreach (Transform child in transform)
        {
            var prefabs = child.GetComponent<T>();
            if(prefabs!= null) poolPrefabs.Add(prefabs);
        }
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
