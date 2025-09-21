using UnityEditor.Rendering;
using UnityEngine;

public class GridPosition : MyNetWorkMonoBehaviour
{
    [SerializeField] private int x;

    [SerializeField] private int y;
    private void OnMouseDown()
    {
        Debug.Log("Click " + gameObject.name);
        GameManager.Instance.ClickedOnGridPositionRpc(this,x, y);
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
