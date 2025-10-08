using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvent : MyNetWorkMonoBehaviour
{

    public static event EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;
    public static event EventHandler OnGameStarted;
    public static event EventHandler OnCurrentPlayerChanged;

    public static event Action<object,WinResult> OnWinGame;
    public class OnClickGridPositionEventArgs : EventArgs
    {
        public Vector3 vtPos;
        public Vector3 vtScale;
        public EnumPlayerType playerType;
    }

    public static void ClickedOnGridPosition(object obj,Vector3 vtPos ,Vector3 vtScale , EnumPlayerType localPlayerType)
    {
        OnClickGridPosition?.Invoke(obj, new OnClickGridPositionEventArgs
        {
            vtPos = vtPos,
            vtScale = vtScale,
            playerType = localPlayerType,
        });
    }

    public static void PlayOnGameStarted(object sender,EventArgs e)
    {
        OnGameStarted?.Invoke(sender, e);
    }

    public static void PlayOnCurrentPlayerChanged(object sender,EventArgs e)
    {
        OnCurrentPlayerChanged?.Invoke(sender, e);
    }

    public static void WinGame(object sender,WinResult WinResult)
    {
        OnWinGame?.Invoke(sender, WinResult);
    }
}

