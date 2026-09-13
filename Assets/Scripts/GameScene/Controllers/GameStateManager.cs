using UnityEngine;

public enum GameState
{
    Playing,
    Paused,
    GameOver
}

// Manages game state (Playing/Paused/GameOver) and time scale.
// Listens to request events and broadcasts state changes.

public class GameStateManager : MonoBehaviour
{
    private static GameState currentState = GameState.Playing;

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

    // Sets the game to playing state (timeScale = 1)
    public static void SetPlaying()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        GameEvents.OnGameStateChanged(GameState.Playing);
        Debug.Log("GameState: Playing (timeScale = 1)");
    }

    // Sets the game to paused state (timeScale = 0)
    // Will not pause if game is already over
    public static void SetPaused()
    {
        if (currentState == GameState.GameOver)
        {
            Debug.Log("Cannot pause during game over state");
            return;
        }
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        GameEvents.OnGameStateChanged(GameState.Paused);
        Debug.Log("GameState: Paused (timeScale = 0)");
    }

    // Sets the game to game over state (timeScale = 0)
    public static void SetGameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        GameEvents.OnGameStateChanged(GameState.GameOver);
        Debug.Log("GameState: GameOver (timeScale = 0)");
    }

    // Toggles between paused and playing states
    // Will not toggle if game is over
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

    // Resumes from pause (returns to playing state)
    public static void Resume()
    {
        if (currentState == GameState.Paused)
        {
            SetPlaying();
        }
    }

    // Resets the game state to playing (used when restarting)
    public static void ResetState()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        GameEvents.OnGameStateChanged(GameState.Playing);
        Debug.Log("GameState: Reset to Playing (timeScale = 1)");
    }

    // Gets the current game state
    public static GameState GetCurrentState()
    {
        return currentState;
    }

    // Force sets time scale (for dev)
    public static void ForceTimeScale(float scale)
    {
        Time.timeScale = scale;
        Debug.LogWarning($"Time.timeScale force set to {scale} outside of state management");
    }
}