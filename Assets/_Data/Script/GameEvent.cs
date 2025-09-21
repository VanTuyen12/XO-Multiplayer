using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvent : MyNetWorkMonoBehaviour
{

    public static event EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;
    public class OnClickGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
        public EnumPlayerType playerType;
    }

    public static void ClickedOnGridPosition(object obj,int xPoint ,int yPoint , EnumPlayerType localPlayerType)
    {
        OnClickGridPosition?.Invoke(obj, new OnClickGridPositionEventArgs
        {
            x = xPoint,
            y = yPoint,
            playerType = localPlayerType,
        });
    }

}

