using UnityEngine;
using Unity.Netcode;

public class StartHostBtn : ButtonAbstract
{
    protected override void OnClick()
    {
        NetworkManager.Singleton.StartHost();
        _button.enabled = false;
       
    }
}
