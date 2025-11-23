using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        // Use GameStateManager to reset state if available, otherwise reset Time.timeScale directly
        if (GameStateManager.instance != null)
        {
            GameStateManager.ResetState();
        }
        else
        {
            Time.timeScale = 1f;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}