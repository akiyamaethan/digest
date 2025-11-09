using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitch: MonoBehaviour
{
    public void SwitchToGame()
    {
        SceneManager.LoadScene("GameScene");
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.UnloadSceneAsync("LoreScreen");
        
    }

    public void SwitchToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
        SceneManager.UnloadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("LoreScreen");
        
    }

    public void SwitchToLore()
    {
        SceneManager.LoadScene("LoreScreen");
        SceneManager.UnloadSceneAsync("TitleScreen");
        SceneManager.UnloadSceneAsync("GameScene");
        
    }
}
