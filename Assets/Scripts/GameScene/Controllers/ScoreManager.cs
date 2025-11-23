using UnityEngine;

public class ScoreManager : SingletonNoPersist<ScoreManager>
{
    private int _score = 0;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
        GameEvents.onScoreGain += updateScore;
    }

    protected override void OnDestroy()
    {
        GameEvents.onScoreGain -= updateScore;
        base.OnDestroy();  // Clean up singleton reference
    }

    public void updateScore(int score)
    {
        _score += score;
        if (ScoreBehavior.instance != null)
        {
            ScoreBehavior.instance.updateScore(_score);
        }
        else
        {
            Debug.LogWarning("[ScoreManager] ScoreBehavior instance not found!");
        }
    }
}
