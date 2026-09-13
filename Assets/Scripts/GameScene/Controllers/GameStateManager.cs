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
    public static bool IsPaused => currentState == GameState.Paused;
    public static bool IsGameOver => currentState == GameState.GameOver;

    void Awake()
    {
        // Subscribe to request events
        GameEvents.onPauseRequested += HandlePauseRequested;
        GameEvents.onResumeRequested += HandleResumeRequested;
        GameEvents.onResetStateRequested += SetPlaying;
        GameEvents.onGameOver += SetGameOver;

        // Ensure we start with normal time scale
        Time.timeScale = 1f;
        currentState = GameState.Playing;
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        GameEvents.onPauseRequested -= HandlePauseRequested;
        GameEvents.onResumeRequested -= HandleResumeRequested;
        GameEvents.onResetStateRequested -= SetPlaying;
        GameEvents.onGameOver -= SetGameOver;

        // Ensure time scale is reset
        Time.timeScale = 1f;
    }

    private void HandlePauseRequested()
    {
        if (!IsGameOver)
            SetPaused();
    }

    private void HandleResumeRequested()
    {
        if (IsPaused)
            SetPlaying();
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
}