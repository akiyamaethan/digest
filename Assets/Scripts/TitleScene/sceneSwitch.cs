using UnityEngine;
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
