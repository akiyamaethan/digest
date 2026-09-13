using TMPro;
using UnityEngine;

// UI component that displays the current score.
public class ScoreBehavior : MonoBehaviour
{
    private TMP_Text scoreValue;

    void Awake()
    {
        GameEvents.onScoreUpdated += HandleScoreUpdated;
    }

    void OnDestroy()
    {
        GameEvents.onScoreUpdated -= HandleScoreUpdated;
    }

    void Start()
    {
        scoreValue = GetComponent<TMP_Text>();
        scoreValue.text = "Score: 0";
    }

    private void HandleScoreUpdated(int totalScore)
    {
        if (scoreValue != null)
            scoreValue.text = "Score: " + totalScore.ToString();
    }
}
