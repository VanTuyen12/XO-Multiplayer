using UnityEngine;

public abstract class MusicCtrl : SoundCtrl
{
    protected override void Reset()
    {
        base.Reset();
        audioSource.loop = true;
    }
}
