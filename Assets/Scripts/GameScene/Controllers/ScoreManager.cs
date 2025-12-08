using UnityEngine;

/// <summary>
/// Manages the current score for the game session.
/// Listens to score gain events and broadcasts score updates.
/// No singleton access required - all communication via events.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    private int _score = 0;

    void Awake()
    {
        GameEvents.onScoreGain += HandleScoreGain;
        GameEvents.onCheckHighScore += HandleCheckHighScore;
    }

    void OnDestroy()
    {
        GameEvents.onScoreGain -= HandleScoreGain;
        GameEvents.onCheckHighScore -= HandleCheckHighScore;
    }

    private void HandleScoreGain(int amount)
    {
        _score += amount;
        // Broadcast updated total score for UI
        GameEvents.OnScoreUpdated(_score);
    }

    private void HandleCheckHighScore()
    {
        HighScoreManager.TrySetHighScore(_score);
    }
}
