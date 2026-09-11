using UnityEngine;

/// Handles pause menu toggling via Escape key and synchronizes UI with GameStateManager.
public class PauseControl : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool isGameOver = false;

    public bool isPaused => GameStateManager.IsPaused;

    void Awake()
    {
        GameEvents.onGameStateChanged += HandleGameStateChanged;
    }

    void OnDestroy()
    {
        GameEvents.onGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState newState)
    {
        isGameOver = (newState == GameState.GameOver);
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(newState == GameState.Paused);
        }
    }

    void Update()
    {
        // Only allow pause toggle if not in game over state
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (GameStateManager.IsPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        GameEvents.OnPauseRequested();
    }

    public void Unpause()
    {
        GameEvents.OnResumeRequested();
    }
}
