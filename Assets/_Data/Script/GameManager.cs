using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public virtual void ClickedOnGridPosition(object obj, int x, int y)
    {
        Debug.Log("ClickedOnGridPosition" + x + "-" + y);
        GameEvent.ClickedOnGridPosition(obj, x, y);
    }
}