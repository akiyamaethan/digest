using UnityEngine;
using UnityEngine.SceneManagement;

public class UnPause : MonoBehaviour
{
    public void UnPauseGame()
    {
        PauseControl pauseControl = FindFirstObjectByType<PauseControl>();
        if (pauseControl != null)
        {
            pauseControl.Unpause();
        }
        else
        {
            Debug.LogError("PauseControl instance not found in the scene.");
        }
    }
}
