using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void SwitchToGame()
    {
        SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);
    }

    public void SwitchToTitle()
    {
        PauseControl pauseControl = FindFirstObjectByType<PauseControl>();
        if (pauseControl != null)
        {
            pauseControl.Unpause();
        }
        SceneManager.LoadSceneAsync("TitleScreen", LoadSceneMode.Single);
        Destroy(GameObject.FindGameObjectWithTag("GameController"));
    }

    public void SwitchToLore()
    {
        SceneManager.LoadSceneAsync("LoreScreen", LoadSceneMode.Single);
    }
}
