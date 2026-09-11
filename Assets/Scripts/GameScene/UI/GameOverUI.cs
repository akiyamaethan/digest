using TMPro;
using UnityEngine;

/// Controls the Game Over screen presentation and displays the appropriate cause of death (starvation vs caught).
public class GameOverUI : MonoBehaviour
{
    [Header("Game Over Displays")]
    [SerializeField] private TMP_Text gameOverTitle;
    [SerializeField] private TMP_Text youStarvedText;
    [SerializeField] private TMP_Text youGotCaughtText;

    [Header("Buttons & Details")]
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject titleButton;
    [SerializeField] private GameObject gameOverHighScore;

    private void Awake()
    {
        GameEvents.onGameOverWithCause += HandleGameOverWithCause;
        GameEvents.onGameOver += HandleGameOverGeneric;
        GameEvents.onHungerDepleted += HandleHungerDepleted;
    }

    private void OnDestroy()
    {
        GameEvents.onGameOverWithCause -= HandleGameOverWithCause;
        GameEvents.onGameOver -= HandleGameOverGeneric;
        GameEvents.onHungerDepleted -= HandleHungerDepleted;
    }

    private void HandleHungerDepleted()
    {
        // When hunger depletes, trigger high score check and game over
        GameEvents.OnCheckHighScore();
        GameEvents.OnGameOver(GameOverCause.Starved);
    }

    private void HandleGameOverGeneric()
    {
        ShowGameOver(GameEvents.lastGameOverCause);
    }

    private void HandleGameOverWithCause(GameOverCause cause)
    {
        ShowGameOver(cause);
    }

    public void ShowGameOver(GameOverCause cause)
    {
        if (gameOverTitle != null)
            gameOverTitle.gameObject.SetActive(true);

        if (cause == GameOverCause.Starved)
        {
            if (youStarvedText != null) youStarvedText.gameObject.SetActive(true);
            if (youGotCaughtText != null) youGotCaughtText.gameObject.SetActive(false);
        }
        else
        {
            if (youGotCaughtText != null) youGotCaughtText.gameObject.SetActive(true);
            if (youStarvedText != null) youStarvedText.gameObject.SetActive(false);
        }

        if (restartButton != null)
            restartButton.SetActive(true);

        if (titleButton != null)
            titleButton.SetActive(true);

        if (gameOverHighScore != null)
            gameOverHighScore.SetActive(true);
    }
}
