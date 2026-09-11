using UnityEngine;
using System;


/// Manages high score persistence using PlayerPrefs.
/// Data persists via PlayerPrefs. Uses static methods for simple access.
public static class HighScoreManager
{
    private const string HIGH_SCORE_KEY = "highScore";
    private static int _highScore;
    private static bool _hasLoaded = false;

    /// Event fired when high score is updated. Passes the new high score value.
    public static event Action<int> onHighScoreUpdated;

    /// Gets the current high score.
    public static int GetHighScore()
    {
        EnsureLoaded();
        return _highScore;
    }

    /// Attempts to update the high score. Only updates if newScore is higher.
    /// Returns true if a new high score was set.
    public static bool TrySetHighScore(int newScore)
    {
        EnsureLoaded();

        if (newScore > _highScore)
        {
            _highScore = newScore;
            SaveHighScore();
            onHighScoreUpdated?.Invoke(_highScore);
            Debug.Log($"[HighScoreManager] New high score: {_highScore}");
            return true;
        }
        return false;
    }

    private static void EnsureLoaded()
    {
        if (_hasLoaded) return;

        _highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        _hasLoaded = true;
        Debug.Log($"[HighScoreManager] Loaded high score: {_highScore}");
    }

    private static void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, _highScore);
        PlayerPrefs.Save();
    }

    /// Resets the high score to 0. Use for debugging or player request.
    public static void ResetHighScore()
    {
        _highScore = 0;
        SaveHighScore();
        onHighScoreUpdated?.Invoke(0);
        Debug.Log("[HighScoreManager] High score reset to 0");
    }
}
