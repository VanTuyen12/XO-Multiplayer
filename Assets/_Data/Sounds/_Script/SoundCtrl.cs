using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundCtrl : PoolObj
{
    [SerializeField]protected AudioSource audioSource;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAudioSource();
    }

    private void LoadAudioSource()
    {
        if (audioSource != null) return;
        audioSource = GetComponent<AudioSource>();
        Debug.Log(transform.name +":LoadAudioSource ",gameObject);
    }
}
