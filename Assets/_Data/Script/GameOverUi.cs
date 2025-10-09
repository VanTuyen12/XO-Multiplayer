using System;
using TMPro;
using UnityEngine;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTextMesh;
    [SerializeField] private Color colorWin;
    [SerializeField] private Color colorLoss;

    private void Awake()
    {
        if (resultTextMesh != null) return;
        resultTextMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
        
    }
    
    private void Start()
    {
        GameEvent.OnWinGame += GameEventOnWinGame;
        Hide();
    }

    private void GameEventOnWinGame(object sender, WinResult winResult)
    {
        if (winResult.winner == GameManager.Instance.GetLocalPlayerType() )
        {
            resultTextMesh.text = "You win!";
            resultTextMesh.color = colorWin;
        }
        else
        {
            resultTextMesh.text = "You lose!";
            resultTextMesh.color = colorLoss;
        }
        Show();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
