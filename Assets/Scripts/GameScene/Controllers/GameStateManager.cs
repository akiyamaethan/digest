using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

/// <summary>
/// Manages game state (Playing/Paused/GameOver) and time scale.
/// Listens to request events and broadcasts state changes.
/// Other systems should subscribe to onGameStateChanged rather than polling.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    private static GameState currentState = GameState.Playing;
    private static GameState previousState = GameState.Playing;

    // Properties to check current state (for systems that cache state via events)
    public static bool IsPlaying => currentState == GameState.Playing;
    public static bool IsPaused => currentState == GameState.Paused;
    public static bool IsGameOver => currentState == GameState.GameOver;

    void Awake()
    {
        // Subscribe to request events
        GameEvents.onPauseRequested += HandlePauseRequested;
        GameEvents.onResumeRequested += HandleResumeRequested;
        GameEvents.onResetStateRequested += HandleResetStateRequested;
        GameEvents.onGameOver += HandleGameOver;

        // Ensure we start with normal time scale
        Time.timeScale = 1f;
        currentState = GameState.Playing;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        GameEvents.onPauseRequested -= HandlePauseRequested;
        GameEvents.onResumeRequested -= HandleResumeRequested;
        GameEvents.onResetStateRequested -= HandleResetStateRequested;
        GameEvents.onGameOver -= HandleGameOver;

        // Ensure time scale is reset
        Time.timeScale = 1f;
    }

    private void HandlePauseRequested()
    {
        SetPaused();
    }

    private void HandleResumeRequested()
    {
        Resume();
    }

    private void HandleResetStateRequested()
    {
        ResetState();
    }

    private void HandleGameOver()
    {
        SetGameOver();
    }

    /// <summary>
    /// Sets the game to playing state (timeScale = 1)
    /// </summary>
    public static void SetPlaying()
    {
        previousState = currentState;
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        GameEvents.OnGameStateChanged(GameState.Playing);
        Debug.Log("GameState: Playing (timeScale = 1)");
    }

    /// <summary>
    /// Sets the game to paused state (timeScale = 0)
    /// Will not pause if game is already over
    /// </summary>
    public static void SetPaused()
    {
        // Don't allow pausing during game over
        if (currentState == GameState.GameOver)
        {
            Debug.Log("Cannot pause during game over state");
            return;
        }

        previousState = currentState;
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        GameEvents.OnGameStateChanged(GameState.Paused);
        Debug.Log("GameState: Paused (timeScale = 0)");
    }

    /// <summary>
    /// Sets the game to game over state (timeScale = 0)
    /// </summary>
    public static void SetGameOver()
    {
        previousState = currentState;
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        GameEvents.OnGameStateChanged(GameState.GameOver);
        Debug.Log("GameState: GameOver (timeScale = 0)");
    }

    /// <summary>
    /// Toggles between paused and playing states
    /// Will not toggle if game is over
    /// </summary>
    public static void TogglePause()
    {
        if (currentState == GameState.GameOver)
        {
            Debug.Log("Cannot toggle pause during game over state");
            return;
        }

        if (currentState == GameState.Paused)
        {
            SetPlaying();
        }
        else
        {
            SetPaused();
        }
    }

    /// <summary>
    /// Resumes from pause (returns to playing state)
    /// </summary>
    public static void Resume()
    {
        if (currentState == GameState.Paused)
        {
            SetPlaying();
        }
    }

    /// <summary>
    /// Resets the game state to playing (used when restarting)
    /// </summary>
    public static void ResetState()
    {
        currentState = GameState.Playing;
        previousState = GameState.Playing;
        Time.timeScale = 1f;
        GameEvents.OnGameStateChanged(GameState.Playing);
        Debug.Log("GameState: Reset to Playing (timeScale = 1)");
    }

    /// <summary>
    /// Gets the current game state
    /// </summary>
    public static GameState GetCurrentState()
    {
        return currentState;
    }

    /// <summary>
    /// Force sets time scale (use with caution, prefer state methods)
    /// </summary>
    public static void ForceTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.LogWarning($"Time.timeScale force set to {scale} outside of state management");
    }
}