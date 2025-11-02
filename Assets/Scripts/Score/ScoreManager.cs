using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
        else
            Destroy(gameObject);
    }

    public void updateScore(int score)
    {
        if (ScoreBehavior.instance == null)
        {
            Debug.LogWarning("ScoreBehavior not initialized!");
            return;
        }
        ScoreBehavior.instance.updateScore(score);
    }
}
