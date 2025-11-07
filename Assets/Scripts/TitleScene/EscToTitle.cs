using UnityEngine;
using UnityEngine.SceneManagement;

public class EscToTitle : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScreen");
        }
    }
}
