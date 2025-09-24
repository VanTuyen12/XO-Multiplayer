using UnityEngine;
using Unity.Netcode;

public class NetworkManagerUI : MyNetWorkMonoBehaviour
{
    protected override void Start()
    {
        base.Start();
        LoadConnectPlay();
    }

    protected virtual void LoadConnectPlay()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            if (NetworkManager.Singleton.IsServer)
            {
                Hide();
            }
        };

        NetworkManager.Singleton.OnClientConnectedCallback += clientIde =>
        {
            if (clientIde == NetworkManager.Singleton.LocalClientId)
            {
                Hide();
            }
        };
    }
    protected virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
