using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvent : MyMonoBehaviour
{

    public static event EventHandler<OnClickGridPositionEventArgs> OnClickGridPosition;
    public class OnClickGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    public static void ClickedOnGridPosition(object obj,int xPoint , int yPoint )
    {
        OnClickGridPosition?.Invoke(obj, new OnClickGridPositionEventArgs
        {
            x = xPoint,
            y = yPoint,
        });
    }

}

