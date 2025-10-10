using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameOverUi : MyMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTextMesh;
    [SerializeField] private Color winColor;
    [SerializeField] private Color lossColer;
    [SerializeField] private Color tiedColer;
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadResultTextMesh();
    }

    protected virtual void LoadResultTextMesh()
    {
        if (resultTextMesh != null) return;
        resultTextMesh = transform.GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + " :resultTextMesh.text ",gameObject);
    }
    
    protected override void Start()
    {
        GameEvent.OnWinGame += GameEventOnWinGame;
        GameEvent.OnTied += GameEventOnOnTied;
        Hide();
    }

    private void GameEventOnOnTied(object sender, EventArgs e)
    {
        resultTextMesh.text = "Game Tied!";
        resultTextMesh.color = tiedColer;
        Show();
    }

    private void GameEventOnWinGame(object sender, WinResult winResult)
    {
        if (winResult.winner == GameManager.Instance.GetLocalPlayerType() )
        {
            resultTextMesh.text = "You win!";
            resultTextMesh.color = winColor;
        }
        else
        {
            resultTextMesh.text = "You lose!";
            resultTextMesh.color = lossColer;
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

    private void OnDestroy()
    {
        GameEvent.OnWinGame -= GameEventOnWinGame;
        GameEvent.OnTied -= GameEventOnOnTied;
    }
}
