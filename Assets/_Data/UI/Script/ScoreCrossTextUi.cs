
public class ScoreCrossTextUi : AbstractText
{
    public virtual void ScoreCrossText(int score)
    {
        textUi.text = score.ToString();
    }
}
