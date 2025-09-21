using UnityEngine;
using Unity.Netcode;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EnumPlayerType _currentPlayerType;
    [SerializeField] private EnumPlayerType _localPlayerType;
    
    [Rpc(SendTo.Server)]
    public virtual void ClickedOnGridPositionRpc( Vector3 vtPos, Vector3 vtScale,EnumPlayerType playerType)
    {
        Debug.Log("ClickedOnGridPositionRpc");
        if (playerType != _currentPlayerType) return;
        
        GameEvent.ClickedOnGridPosition(null, vtPos, vtScale,playerType);
        switch (_currentPlayerType)
        {
            default:
                case EnumPlayerType.Cross:
                    _currentPlayerType = EnumPlayerType.Circle;
                    break;
                case EnumPlayerType.Circle:
                    _currentPlayerType = EnumPlayerType.Cross;
                    break;
        }
    }
    
    public override void OnNetworkSpawn()
    {
        Debug.Log(NetworkManager.Singleton.LocalClientId);
        _localPlayerType = NetworkManager.Singleton.LocalClientId == 0 ? EnumPlayerType.Cross : EnumPlayerType.Circle;

        if (IsServer)
        {
            _currentPlayerType = EnumPlayerType.Cross;
        }
    }
    
    public virtual EnumPlayerType GetLocalPlayerType()
    {
        return _localPlayerType;
    }
}