using UnityEngine;

public abstract class PoolObj : MyNetWorkMonoBehaviour
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public abstract string GetName();
}
