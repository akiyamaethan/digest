using UnityEngine;

public class PauseControl : SingletonNoPersist<PauseControl>
{
    public bool isPaused = false;
    public GameObject pauseMenu;

    protected override void Awake()
    {
        base.Awake();
    }
    void Update()
    {
        // Only allow pause toggle if not in game over state
        // Check if GameStateManager exists, if not, allow pause anyway
        bool canPause = (GameStateManager.instance == null) || !GameStateManager.IsGameOver;

        if (Input.GetKeyDown(KeyCode.Escape) && canPause)
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

        // Try to use GameStateManager if it exists, otherwise fall back to direct Time.timeScale
        if (GameStateManager.instance != null)
        {
            GameStateManager.SetPaused();
        }
        else
        {
            Time.timeScale = 0f;
            Debug.LogWarning("GameStateManager not found in scene - using direct Time.timeScale manipulation. Add GameStateManager to the scene for proper state management.");
        }
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        isPaused = false;

        // Try to use GameStateManager if it exists, otherwise fall back to direct Time.timeScale
        if (GameStateManager.instance != null)
        {
            GameStateManager.Resume();
        }
        else
        {
            Time.timeScale = 1f;
            Debug.LogWarning("GameStateManager not found in scene - using direct Time.timeScale manipulation. Add GameStateManager to the scene for proper state management.");
        }
    }
}
