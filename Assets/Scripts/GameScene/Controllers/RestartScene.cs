using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene reloading (restart functionality).
/// Uses events for state reset - no singleton access required.
/// </summary>
public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        // Reset game state via event and ensure time scale is reset
        GameEvents.OnResetStateRequested();
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}