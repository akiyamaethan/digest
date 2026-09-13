using UnityEngine;

// Manages the current score for the game session.
// Listens to score gain events and broadcasts score updates.
public class ScoreManager : MonoBehaviour
{
    private int _score = 0;

    void Awake()
    {
        GameEvents.onScoreGain += HandleScoreGain;
        GameEvents.onGameOver += HandleGameOver;
    }

    void OnDestroy()
    {
        GameEvents.onScoreGain -= HandleScoreGain;
        GameEvents.onGameOver -= HandleGameOver;
    }

    private void HandleScoreGain(int amount)
    {
        _score += amount;
        GameEvents.OnScoreUpdated(_score);
    }

    private void HandleGameOver()
    {
        HighScoreManager.TrySetHighScore(_score);
    }
}
