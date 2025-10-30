using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ScoreBehavior : MonoBehaviour
{
    public TMP_Text scoreValue;
    public UnityEvent onScoreChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreValue = GetComponent<TMP_Text>();
        scoreValue.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScore(int score)
    {
        scoreValue.text = score.ToString();
    }
}
