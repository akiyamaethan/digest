using UnityEngine;
using System;

/// <summary>
/// Manages high score persistence using PlayerPrefs.
/// Data persists via PlayerPrefs, not DontDestroyOnLoad.
/// </summary>
public class HighScoreManager : SingletonNoPersist<HighScoreManager>
{
    private const string HIGH_SCORE_KEY = "highScore";

    private int _highScore;
    private bool _hasLoaded = false;

    /// <summary>
    /// Event fired when high score is updated. Passes the new high score value.
    /// </summary>
    public static event Action<int> onHighScoreUpdated;

    protected override void Awake()
    {
        base.Awake();
        LoadHighScore();
    }

    /// <summary>
    /// Gets the current high score.
    /// </summary>
    public static int GetHighScore()
    {
        // Accessing 'instance' will auto-create the singleton if needed
        // Only return 0 if we truly can't get an instance (e.g., app quitting)
        var inst = instance;
        if (inst == null) return 0;

        // Ensure score is loaded even if Awake hasn't run yet
        if (!inst._hasLoaded)
        {
            inst.LoadHighScore();
        }

        return inst._highScore;
    }

    /// <summary>
    /// Attempts to update the high score. Only updates if newScore is higher.
    /// Returns true if a new high score was set.
    /// </summary>
    public static bool TrySetHighScore(int newScore)
    {
        if (instance == null) return false;

        if (newScore > instance._highScore)
        {
            instance._highScore = newScore;
            instance.SaveHighScore();
            onHighScoreUpdated?.Invoke(instance._highScore);
            Debug.Log($"[HighScoreManager] New high score: {instance._highScore}");
            return true;
        }
        return false;
    }

    private void LoadHighScore()
    {
        if (_hasLoaded) return;

        _highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        _hasLoaded = true;
        Debug.Log($"[HighScoreManager] Loaded high score: {_highScore}");
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY, _highScore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Resets the high score to 0. Use for debugging or player request.
    /// </summary>
    public static void ResetHighScore()
    {
        if (instance == null) return;

        instance._highScore = 0;
        instance.SaveHighScore();
        onHighScoreUpdated?.Invoke(0);
        Debug.Log("[HighScoreManager] High score reset to 0");
    }
}
