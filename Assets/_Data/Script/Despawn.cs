using System;
using System.Collections;

using UnityEngine;

public abstract class Despawn<T> : DespawnBase where T : PoolObj
{
    [SerializeField]protected Spawner<T> spawner;
    [SerializeField]protected T parent;
    [SerializeField]protected float timeLife = 5f;
    [SerializeField]protected bool isDespawnByTime = true;
    private Coroutine lifeCoroutine;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSpawner();
        this.LoadParent();
    }

    protected virtual void OnEnable()
    {
        if (!isDespawnByTime) return;
        
        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine);

        lifeCoroutine = StartCoroutine(LifeTimer());
    }
    
    protected virtual IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(timeLife);
        DoDespawn();
    }
    protected virtual void OnDisable()
    {
        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine);
    }
    protected virtual void LoadSpawner()
    {
        if (spawner != null) return;
        spawner = FindAnyObjectByType<Spawner<T>>();
        Debug.Log(transform.name + " :LoadSpawner", gameObject);
    }
    
    protected virtual void LoadParent()
    {
        if (parent != null) return;
        parent = this.transform.parent.GetComponent<T>();
        Debug.Log(transform.name + " :LoadParent", gameObject);
    }
    public override void DoDespawn()
    {
        spawner.Despawn(this.parent);
    }
    
}
