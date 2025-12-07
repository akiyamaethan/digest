using TMPro;
using UnityEngine;

/// <summary>
/// Displays the high score on a TMP_Text component.
/// Supports different display formats for different UI contexts.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class HighScoreDisplay : MonoBehaviour
{
    public enum DisplayFormat
    {
        LoreScreen,    // "Your High Score: {score}"
        GameScreen     // "High Score: {score}"
    }

    [SerializeField] private DisplayFormat format = DisplayFormat.GameScreen;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        HighScoreManager.onHighScoreUpdated += OnHighScoreUpdated;
        UpdateDisplay();
    }

    private void OnDisable()
    {
        HighScoreManager.onHighScoreUpdated -= OnHighScoreUpdated;
    }

    private void OnHighScoreUpdated(int newHighScore)
    {
        UpdateDisplay(newHighScore);
    }

    private void UpdateDisplay()
    {
        int highScore = HighScoreManager.GetHighScore();
        UpdateDisplay(highScore);
    }

    private void UpdateDisplay(int highScore)
    {
        if (_text == null) return;

        switch (format)
        {
            case DisplayFormat.LoreScreen:
                _text.text = $"Your High Score: {highScore}";
                break;
            case DisplayFormat.GameScreen:
                _text.text = $"High Score: {highScore}";
                break;
        }
    }
}
