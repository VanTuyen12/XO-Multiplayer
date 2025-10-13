using UnityEditor.Rendering;
using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int x;

    [SerializeField] private int y;


    private void OnMouseDown()
    {
        var gameManager = GameManager.Instance;
        if (gameManager == null) return;

        gameManager.ClickedOnGridPositionRpc(x, y, transform.position, transform.localScale,
            gameManager.GetLocalPlayerType());
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