using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvent : MyNetWorkMonoBehaviour
{

    public static event EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;
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

}

