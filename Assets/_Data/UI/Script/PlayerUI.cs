using System;
using UnityEngine;

public class PlayerUI : MyNetWorkMonoBehaviour
{
    [SerializeField] private GameObject crossYouText;
    [SerializeField] private GameObject crossArrowImage;
    
    [SerializeField] private GameObject circleYouText;
    [SerializeField] private GameObject circleArrowImage;

    protected override void Awake()
    {
        base.Awake();
        LoadStartPlayGame();
    }

    protected override void Start()
    {
        base.Start();
        GameEvent.OnGameStarted += GameEvent_OnGameStarted;
        GameEvent.OnCurrentPlayerChanged += GameEventOnOnCurrentPlayerChanged;
    }

    protected virtual void GameEventOnOnCurrentPlayerChanged(object sender, EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void GameEvent_OnGameStarted(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetLocalPlayerType() == EnumPlayerType.Cross)
        {
            crossYouText.SetActive(true);
        }
        else
        {
            circleYouText.SetActive(true);
        }

        UpdateCurrentArrow();
    }

    protected virtual void UpdateCurrentArrow()
    {
        if (GameManager.Instance.GetCurrentPlayerType() == EnumPlayerType.Cross)
        {
            crossArrowImage.SetActive(true);
            circleArrowImage.SetActive(false);
        }
        else
        {
            crossArrowImage.SetActive(false);
            circleArrowImage.SetActive(true);
        }
    }

    protected virtual void LoadStartPlayGame()
    {
        crossYouText.SetActive(false);
        circleYouText.SetActive(false);
        circleArrowImage.SetActive(false);
        crossArrowImage.SetActive(false);
    }
}
