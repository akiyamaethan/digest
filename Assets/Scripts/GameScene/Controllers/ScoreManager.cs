using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private int _score = 0;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else
            Destroy(gameObject);
    }

    public void updateScore(int score)
    {
        _score += score;
        ScoreBehavior.instance.updateScore(_score);
    }
}
