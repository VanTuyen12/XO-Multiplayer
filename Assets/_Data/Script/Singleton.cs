using System;
using Unity.Netcode;
using UnityEngine;

public class Singleton<T> : MyNetWorkMonoBehaviour where T : NetworkBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Singleton instance has not been created yet!");
            }
            return _instance;
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        this.LoadInstance();
    }

    private void LoadInstance()
    {
        if (_instance == null)
        {
            _instance = this as T;
            
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
                return;
            }
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }
}