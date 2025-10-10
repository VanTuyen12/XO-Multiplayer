using System;
using Unity.VisualScripting;
using UnityEngine;

public class RematchBtn : ButtonAbstract
{
    [SerializeField] GameOverUi gameOverUi;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameOverUi();
    }

    protected override void Start()
    {
        base.Start();
        GameEvent.OnRematch += GameEventOnRematch;
    }

    private void GameEventOnRematch(object sender, EventArgs e)
    {
        gameOverUi.Hide();
    }

    protected virtual void LoadGameOverUi()
    {
        if (this.gameOverUi != null) return;
            gameOverUi = transform.GetComponentInParent<GameOverUi>();
        Debug.Log(transform.name + " :LoadRematchBtn",gameObject);
    }

    protected override void OnClick()
    {
        GameManager.Instance.RematchRpc();
    }

    protected virtual void OnDestroy()
    {
        GameEvent.OnRematch -= GameEventOnRematch;
    }
}
