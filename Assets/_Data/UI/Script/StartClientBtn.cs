using UnityEngine;
using Unity.Netcode;
public class StartClientBtn : ButtonAbstract
{
    protected override void OnClick()
    {
       NetworkManager.Singleton.StartClient();
       _button.enabled = false;
    }
    
}
