using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwitch: MonoBehaviour
{
    public void SwitchToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SwitchToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void SwitchToLore()
    {
        SceneManager.LoadScene("LoreScreen");
    }
}
