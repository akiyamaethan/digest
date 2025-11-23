using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class ScoreBehavior : SingletonNoPersist<ScoreBehavior>
{
    public TMP_Text scoreValue;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
    }

    void Start()
    {
        scoreValue = GetComponent<TMP_Text>();
        scoreValue.text = "Score: 0";
    }

    public void updateScore(int score)
    {
        scoreValue.text = "Score: " + score.ToString();
    }
}
