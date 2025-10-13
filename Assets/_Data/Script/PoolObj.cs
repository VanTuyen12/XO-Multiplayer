using UnityEngine;

public abstract class PoolObj : MyMonoBehaviour
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public abstract string GetName();
}
