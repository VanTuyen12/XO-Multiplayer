using UnityEngine;

public abstract class Despawn<T> : DespawnBase where T : PoolObj
{
    [SerializeField]protected Spawner<T> spawner;
    [SerializeField]protected T parent;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawner();
        this.LoadParent();
    }

    protected virtual void LoadSpawner()
    {
        if (spawner != null) return;
        spawner = transform.GetComponentInParent<Spawner<T>>();
        Debug.Log(transform.name + " :LoadSpawner", gameObject);
    }
    
    protected virtual void LoadParent()
    {
        if (parent != null) return;
        parent = transform.GetComponent<T>();
        Debug.Log(transform.name + " :LoadParent", gameObject);
    }
    protected override void DoDespawn()
    {
        spawner.Despawn(this.parent);
    }
}
