using UnityEngine;

/// Handles pause menu toggling via Escape key.
public class PauseControl : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    private bool isGameOver = false;

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
    }

    void Update()
    {
        // Only allow pause toggle if not in game over state
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        GameEvents.OnPauseRequested();
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        GameEvents.OnResumeRequested();
    }
}
