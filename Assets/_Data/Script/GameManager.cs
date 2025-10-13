using System;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private NetworkVariable<EnumPlayerType> currentPlayerType = new();
    [SerializeField] private EnumPlayerType _localPlayerType;
    private EnumPlayerType[,] _playerTypesArray;

    //Game Over
    [SerializeField] private NetworkVariable<bool> isGameOver = new(false);
    
    //Score Player
    [SerializeField] private NetworkVariable<int> scoreCrossPlayer = new(0);
    [SerializeField] private NetworkVariable<int> scoreCirclePlayer = new(0);

    public EnumPlayerType[,] PlayerTypesArray
    {
        get => _playerTypesArray;
        set => _playerTypesArray = value;
    }

    [Rpc(SendTo.Server)]
    public virtual void ClickedOnGridPositionRpc(int x, int y, Vector3 vtPos, Vector3 vtScale,
        EnumPlayerType playerType)
    {
        if (isGameOver.Value) return;

        if (playerType != currentPlayerType.Value) return;
        if (_playerTypesArray[x, y] != EnumPlayerType.None) return;

        _playerTypesArray[x, y] = playerType;
        GameEvent.ClickedOnGridPosition(null, vtPos, vtScale, playerType);

        WinResult winPlayerType = WinChecker.CheckWin(_playerTypesArray, x, y);
        if (winPlayerType.winner != EnumPlayerType.None)
        {
            isGameOver.Value = true;
            SetScorePlayer(winPlayerType.winner);
            OnWinGameRpc(winPlayerType);
            return;
        }

        if (WinChecker.IsBoardFull(_playerTypesArray))
        {
            isGameOver.Value = true;
            GameTiedRpc();
            return;
        }

        SwitchPlayer();
    }

    protected virtual void SetScorePlayer(EnumPlayerType playerType)
    {
        switch (playerType)
        {
            case EnumPlayerType.Cross :
                scoreCrossPlayer.Value++;
                break;
            case EnumPlayerType.Circle:
                scoreCirclePlayer.Value++;
                break;
        }
    }
    [Rpc(SendTo.ClientsAndHost)]
    protected virtual void GameTiedRpc()
    {
        GameEvent.GameTied(this,EventArgs.Empty);
    }
    
    private void SwitchPlayer()
    {
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
    
    [Rpc(SendTo.ClientsAndHost)]
    protected virtual void OnWinGameRpc(WinResult winResult)
    {
        GameEvent.WinGame(this,winResult);
    }

    [Rpc(SendTo.Server)]
    public virtual void RematchRpc()
    {
        isGameOver.Value = false;
        
        for (int i = 0; i < _playerTypesArray.GetLength(0); i++)
        {
            for (int j = 0; j < _playerTypesArray.GetLength(1); j++)
            {
                _playerTypesArray[i, j] = EnumPlayerType.None;
            }
        }
        
        currentPlayerType.Value = EnumPlayerType.Cross;
        TriggerOnRematchRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    protected virtual void TriggerOnRematchRpc()
    {
        GameEvent.Rematch(this, EventArgs.Empty);
    }
    public override void OnNetworkSpawn()
    {
        //Debug.Log(NetworkManager.Singleton.LocalClientId);
        _localPlayerType = NetworkManager.Singleton.LocalClientId == 0 ? EnumPlayerType.Cross : EnumPlayerType.Circle;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SingletonOnOnClientConnectedCallback;
        }

        currentPlayerType.OnValueChanged += (oldPlayerType, newPlayerType) =>
        {
            GameEvent.PlayOnCurrentPlayerChanged(this, EventArgs.Empty);
        };

        scoreCrossPlayer.OnValueChanged += (prevScore, newScore) =>
        {
            GameEvent.ScoreChanged(this,EventArgs.Empty);
        };
        
        scoreCirclePlayer.OnValueChanged += (prevScore, newScore) =>
        {
            GameEvent.ScoreChanged(this,EventArgs.Empty);
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

    public virtual void GetScore(out int scoreCross, out int scoreCircle)
    {
        scoreCross = scoreCrossPlayer.Value;
        scoreCircle = scoreCirclePlayer.Value;
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