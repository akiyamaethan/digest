using UnityEngine;
using UnityEngine.SceneManagement;

// Unified scene management and navigation controller.
// Handles scene transitions, level reloads, time scale resetting, and optional escape-to-title shortcuts.
// Decoupled and event-driven via GameEvents.
public class SceneSwitch : MonoBehaviour
{
    [Header("Shortcut Settings")]
    [Tooltip("If true, pressing Escape will transition back to the Title Screen.")]
    [SerializeField] private bool enableEscapeToTitle = false;

    private const string GAME_SCENE = "GameScene";
    private const string TITLE_SCENE = "TitleScreen";
    private const string LORE_SCENE = "LoreScreen";

    private void Update()
    {
        if (enableEscapeToTitle && Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchToTitle();
        }
    }

    // Starts/transitions to the main gameplay scene.
    public void SwitchToGame()
    {
        ResetStateAndTimescale();
        SceneManager.LoadSceneAsync(GAME_SCENE, LoadSceneMode.Single);
    }

    // Transitions back to the Title Screen.
    public void SwitchToTitle()
    {
        ResetStateAndTimescale();
        GameEvents.OnResumeRequested(); // Ensure unpaused
        SceneManager.LoadSceneAsync(TITLE_SCENE, LoadSceneMode.Single);
    }

    // Transitions to the Lore / Instructions screen.
    public void SwitchToLore()
    {
        SceneManager.LoadSceneAsync(LORE_SCENE, LoadSceneMode.Single);
    }

    // Reloads the currently active scene (for game over / restart button).
    public void ReloadScene()
    {
        ResetStateAndTimescale();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetStateAndTimescale()
    {
        GameEvents.OnResetStateRequested();
        Time.timeScale = 1f;
    }
}

