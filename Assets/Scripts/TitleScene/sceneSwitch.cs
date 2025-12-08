using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene transitions between game scenes.
/// Uses events for state reset - no singleton access required.
/// </summary>
public class SceneSwitch : MonoBehaviour
{
    public void SwitchToGame()
    {
        // Reset game state via event and ensure time scale is reset
        GameEvents.OnResetStateRequested();
        Time.timeScale = 1f;  // Fallback to ensure time scale is reset
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }

    public void SwitchToTitle()
    {
        // Reset game state via event
        GameEvents.OnResetStateRequested();
        GameEvents.OnResumeRequested();  // Unpause if paused
        Time.timeScale = 1f;  // Fallback to ensure time scale is reset

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
