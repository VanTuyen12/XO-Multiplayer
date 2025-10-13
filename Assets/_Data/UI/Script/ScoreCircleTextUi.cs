
public class ScoreCircleTextUi : AbstractText
{
    public virtual void ScoreCircleText(int score)
    {
        textUi.text = score.ToString();
    }
}
