using UnityEditor.Rendering;
using UnityEngine;

public class GridPosition : MyNetWorkMonoBehaviour
{
    [SerializeField] private int x;

    [SerializeField] private int y;
    private void OnMouseDown()
    {
        Debug.Log("Click " + gameObject.name);
        
        var gameManager = GameManager.Instance;
        if (gameManager == null) return;
        gameManager.ClickedOnGridPositionRpc(transform.position, transform.localScale,gameManager.GetLocalPlayerType());
    }
    
    public void SetPosX(int x)
    {
        this.x = x;
    }

    public void SetPosY(int y)
    {
        this.y = y;
    }
}
