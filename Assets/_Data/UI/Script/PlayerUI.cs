using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MyNetWorkMonoBehaviour
{
    [SerializeField] private GameObject crossYouText;
    [SerializeField] private GameObject crossArrowImage;
    
    [SerializeField] private GameObject circleYouText;
    [SerializeField] private GameObject circleArrowImage;
    
    [SerializeField] private ScoreCrossTextUi scoreCrossText;
    [SerializeField] private ScoreCircleTextUi scoreCircleText;

    protected override void Awake()
    {
        base.Awake();
        LoadStartPlayGame();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGameObjetStart();
        this.LoadScoreTextUi();
    }

    protected virtual void LoadGameObjetStart()
    {
        if (crossYouText!= null) return;
        crossYouText = transform.Find("CrossYouTxt").gameObject;

        if (crossArrowImage != null) return;
        crossArrowImage = transform.Find("CrossArrowImage").gameObject;
        
        if (circleYouText!= null) return;
        circleYouText = transform.Find("CircleYouTxt").gameObject;

        if (circleArrowImage != null) return;
        circleArrowImage = transform.Find("CircleArrowImage").gameObject;
        
        Debug.Log(transform.name+ ":LoadGameObjetStart",gameObject);
    }

    protected virtual void LoadScoreTextUi()
    {
        if (scoreCrossText != null) return;
        scoreCrossText = GetComponentInChildren<ScoreCrossTextUi>();

        if (scoreCircleText != null) return;
        scoreCircleText = GetComponentInChildren<ScoreCircleTextUi>();
        
        Debug.Log(transform.name+ ":ScoreTextUi",gameObject);
    }

    protected override void Start()
    {
        base.Start();
        GameEvent.OnGameStarted += GameEvent_OnGameStarted;
        GameEvent.OnCurrentPlayerChanged += GameEventOnOnCurrentPlayerChanged;
        GameEvent.OnScoreChanged += GameEventOnScoreChanged;
    }

    private void GameEventOnScoreChanged(object sender, EventArgs e)
    {
        GameManager.Instance.GetScore(out int scoreCross, out int scoreCircle);
        scoreCircleText.ScoreCircleText(scoreCircle);
        scoreCrossText.ScoreCrossText(scoreCross);
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
        
        scoreCircleText.ScoreCircleText(0);
        scoreCrossText.ScoreCrossText(0);
        
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

    public override void OnDestroy()
    {
        base.OnDestroy();
        GameEvent.OnGameStarted -= GameEvent_OnGameStarted;
        GameEvent.OnCurrentPlayerChanged -= GameEventOnOnCurrentPlayerChanged;
    }
}
