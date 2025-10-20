using UnityEngine;

public class ObjectDespawn : Despawn<ObjectPrefabsCtrl>
{
    protected override void Reset()
    {
        base.Reset();
        isDespawnByTime = false;
    }
}
