using UnityEngine;

public class SoundPool : PoolPrefabs<SoundCtrl>
{
    
    
    protected override void Reset()
    {
        resourcePath = "Sounds";
        base.Reset();
       
    }
    
}
