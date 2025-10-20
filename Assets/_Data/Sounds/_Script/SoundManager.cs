using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]protected MusicCtrl musicCtrl;
    [SerializeField]protected SFXCtrl sfxCtrl;
    [SerializeField]protected SoundSpawn soundSpawn;
    [SerializeField]private List<MusicCtrl> listMusic = new List<MusicCtrl>();
    [SerializeField]private List<SFXCtrl> listSfx = new List<SFXCtrl>();
    
    protected override void Awake()
    {
        base.Awake();
        GameEvent.OnPlacedObject += GameEventOnPlacedObject;
        GameEvent.OnWinGame += GameEventOnWinGame;
    }

    protected virtual void GameEventOnWinGame(object obj, WinResult winResult)
    {
        if (GameManager.Instance.GetLocalPlayerType() == winResult.winner)
        {
            sfxCtrl = CreateSfx(SoundName.SFXWin);
        }
        else
        {
            sfxCtrl = CreateSfx(SoundName.SFXLose);
        }
       
    }
    

    private void GameEventOnPlacedObject(object sender, EventArgs e)
    {
        sfxCtrl = CreateSfx(SoundName.SFXPiacingObj);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSoundSpawn();
    }
    
    protected virtual void LoadSoundSpawn()
    {
        if (soundSpawn != null) return;
        soundSpawn = GetComponent<SoundSpawn>();
        Debug.Log(transform.name + " :LoadSoundSpawn", gameObject);
    }

    public virtual SFXCtrl CreateSfx(SoundName soundName)
    {
        var soundPrefab = soundSpawn.PoolPrefabs.GetByName(soundName.ToString());
        return CreateSfx(soundPrefab);
    }

    public virtual SFXCtrl CreateSfx(SoundCtrl soundPrefab)
    {
        var newSound = (SFXCtrl)soundSpawn.Spawn(soundPrefab, Vector3.zero);
        if (newSound != null) AddSfx(newSound) ;
        return newSound;
    }
    
    protected virtual void AddSfx(SFXCtrl newSound)
    {
        if (this.listSfx.Contains(newSound)) return;
        this.listSfx.Add(newSound);
    }
    
    public virtual SFXCtrl CreateMusic(SoundName soundName)
    {
        var soundPrefab = soundSpawn.PoolPrefabs.GetByName(soundName.ToString());
        return CreateMusic(soundPrefab);
    }

    public virtual SFXCtrl CreateMusic(SoundCtrl soundPrefab)
    {
        var newSound = (SFXCtrl)soundSpawn.Spawn(soundPrefab, Vector3.zero);
        if (newSound != null) AddMusic(newSound) ;
        return newSound;
    }
    
    protected virtual void AddMusic(SFXCtrl newSound)
    {
        if (this.listSfx.Contains(newSound)) return;
        this.listSfx.Add(newSound);
    }

    public new void OnDestroy()
    {
        GameEvent.OnPlacedObject -= GameEventOnPlacedObject;
    }
}
