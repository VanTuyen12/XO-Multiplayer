using System;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private NetworkVariable<EnumPlayerType> currentPlayerType = new();
    [SerializeField] private EnumPlayerType _localPlayerType;
    [SerializeField] private EnumPlayerType[,] _playerTypesArray;

    public EnumPlayerType[,] PlayerTypesArray
    {
        get => _playerTypesArray;
        set => _playerTypesArray = value;
    }

    [Rpc(SendTo.Server)]
    public virtual void ClickedOnGridPositionRpc(int x, int y, Vector3 vtPos, Vector3 vtScale, EnumPlayerType playerType)
    {
        Debug.Log("ClickedOnGridPositionRpc");
        if (playerType != currentPlayerType.Value) return;
        if (_playerTypesArray[x, y] != EnumPlayerType.None) return;

        _playerTypesArray[x, y] = playerType;
        GameEvent.ClickedOnGridPosition(null, vtPos, vtScale, playerType);
        switch (currentPlayerType.Value)
        {
            default:
            case EnumPlayerType.Cross:
                currentPlayerType.Value = EnumPlayerType.Circle;
                break;
            case EnumPlayerType.Circle:
                currentPlayerType.Value = EnumPlayerType.Cross;
                break;
        }
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(NetworkManager.Singleton.LocalClientId);
        _localPlayerType = NetworkManager.Singleton.LocalClientId == 0 ? EnumPlayerType.Cross : EnumPlayerType.Circle;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
        }

        currentPlayerType.OnValueChanged += (oldPlayerType, newPlayerType) =>
        {
            GameEvent.PlayOnCurrentPlayerChanged(this, EventArgs.Empty);
        };
    }

    private void SingletonOnOnClientConnectedCallback(ulong obj)
    {
        if (NetworkManager.Singleton.ConnectedClientsList.Count != 2) return;
        currentPlayerType.Value = EnumPlayerType.Cross;
        TriggerOnGameStartedRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    protected virtual void TriggerOnGameStartedRpc()
    {
        GameEvent.PlayOnGameStarted(this, EventArgs.Empty);
    }

    public virtual EnumPlayerType GetLocalPlayerType()
    {
        return _localPlayerType;
    }

    public virtual EnumPlayerType GetCurrentPlayerType()
    {
        return currentPlayerType.Value;
    }
}