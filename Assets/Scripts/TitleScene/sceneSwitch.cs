using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Unified scene management and navigation controller.
/// Handles scene transitions, level reloads, time scale resetting, and optional escape-to-title shortcuts.
/// Decoupled and event-driven via GameEvents.
/// </summary>
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

    /// <summary>
    /// Starts/transitions to the main gameplay scene.
    /// </summary>
    public void SwitchToGame()
    {
        ResetStateAndTimescale();
        SceneManager.LoadSceneAsync(GAME_SCENE, LoadSceneMode.Single);
    }

    /// <summary>
    /// Transitions back to the Title Screen.
    /// </summary>
    public void SwitchToTitle()
    {
        ResetStateAndTimescale();
        GameEvents.OnResumeRequested(); // Ensure unpaused
        SceneManager.LoadSceneAsync(TITLE_SCENE, LoadSceneMode.Single);
    }

    /// <summary>
    /// Transitions to the Lore / Instructions screen.
    /// </summary>
    public void SwitchToLore()
    {
        SceneManager.LoadSceneAsync(LORE_SCENE, LoadSceneMode.Single);
    }

    /// <summary>
    /// Reloads the currently active scene (for game over / restart button).
    /// </summary>
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

