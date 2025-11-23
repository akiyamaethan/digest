using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void SwitchToGame()
    {
        // Reset game state when starting a new game (if GameStateManager exists)
        if (GameStateManager.instance != null)
        {
            GameStateManager.ResetState();
        }
        else
        {
            Time.timeScale = 1f;  // Fallback to ensure time scale is reset
        }
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }

    public void SwitchToTitle()
    {
        // Reset game state when returning to title (if GameStateManager exists)
        if (GameStateManager.instance != null)
        {
            GameStateManager.ResetState();
        }
        else
        {
            Time.timeScale = 1f;  // Fallback to ensure time scale is reset
        }

        PauseControl pauseControl = FindFirstObjectByType<PauseControl>();
        if (pauseControl != null)
        {
            pauseControl.Unpause();
        }
        SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        if (gameController != null)
        {
            Destroy(gameController);
        }
    }

    public void SwitchToLore()
    {
        SceneManager.LoadSceneAsync("LoreScreen", LoadSceneMode.Single);
    }
}
