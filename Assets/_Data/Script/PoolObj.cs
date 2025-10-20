using UnityEngine;

public abstract class PoolObj : MyNetWorkMonoBehaviour
{
    [SerializeField] protected DespawnBase despawn;
    public DespawnBase Despawn => despawn;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadDespawnBase();
    }

    protected virtual void LoadDespawnBase()
    {
       if(despawn != null) return;
       despawn = transform.GetComponentInChildren<DespawnBase>();
       Debug.Log(transform.name + "LoadDespawn",gameObject);
    }

    public abstract string GetName();
}
