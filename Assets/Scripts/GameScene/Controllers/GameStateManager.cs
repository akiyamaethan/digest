using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

public class GameStateManager : SingletonNoPersist<GameStateManager>
{
    private GameState currentState = GameState.Playing;
    private GameState previousState = GameState.Playing;

    // Properties to check current state
    public static bool IsPlaying => instance != null && instance.currentState == GameState.Playing;
    public static bool IsPaused => instance != null && instance.currentState == GameState.Paused;
    public static bool IsGameOver => instance != null && instance.currentState == GameState.GameOver;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic

        // Ensure we start with normal time scale
        Time.timeScale = 1f;
        currentState = GameState.Playing;
    }

    /// <summary>
    /// Sets the game to playing state (timeScale = 1)
    /// </summary>
    public static void SetPlaying()
    {
        if (instance == null) return;

        instance.previousState = instance.currentState;
        instance.currentState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("GameState: Playing (timeScale = 1)");
    }

    /// <summary>
    /// Sets the game to paused state (timeScale = 0)
    /// Will not pause if game is already over
    /// </summary>
    public static void SetPaused()
    {
        if (instance == null) return;

        // Don't allow pausing during game over
        if (instance.currentState == GameState.GameOver)
        {
            Debug.Log("Cannot pause during game over state");
            return;
        }

        instance.previousState = instance.currentState;
        instance.currentState = GameState.Paused;
        Time.timeScale = 0f;
        Debug.Log("GameState: Paused (timeScale = 0)");
    }

    /// <summary>
    /// Sets the game to game over state (timeScale = 0)
    /// </summary>
    public static void SetGameOver()
    {
        if (instance == null) return;

        instance.previousState = instance.currentState;
        instance.currentState = GameState.GameOver;
        Time.timeScale = 0f;
        Debug.Log("GameState: GameOver (timeScale = 0)");
    }

    /// <summary>
    /// Toggles between paused and playing states
    /// Will not toggle if game is over
    /// </summary>
    public static void TogglePause()
    {
        if (instance == null) return;

        if (instance.currentState == GameState.GameOver)
        {
            Debug.Log("Cannot toggle pause during game over state");
            return;
        }

        if (instance.currentState == GameState.Paused)
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
        if (instance == null) return;

        if (instance.currentState == GameState.Paused)
        {
            SetPlaying();
        }
    }

    /// <summary>
    /// Resets the game state to playing (used when restarting)
    /// </summary>
    public static void ResetState()
    {
        if (instance == null) return;

        instance.currentState = GameState.Playing;
        instance.previousState = GameState.Playing;
        Time.timeScale = 1f;
        Debug.Log("GameState: Reset to Playing (timeScale = 1)");
    }

    /// <summary>
    /// Gets the current game state
    /// </summary>
    public static GameState GetCurrentState()
    {
        if (instance == null) return GameState.Playing;
        return instance.currentState;
    }

    /// <summary>
    /// Force sets time scale (use with caution, prefer state methods)
    /// </summary>
    public static void ForceTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.LogWarning($"Time.timeScale force set to {scale} outside of state management");
    }

    protected override void OnDestroy()
    {
        // Ensure time scale is reset if manager is destroyed
        if (instance == this)
        {
            Time.timeScale = 1f;
        }
        base.OnDestroy();  // Clean up singleton reference
    }
}