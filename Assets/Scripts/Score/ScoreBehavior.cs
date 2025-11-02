using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ScoreBehavior : MonoBehaviour
{
    public TMP_Text scoreValue;
    public static ScoreBehavior instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreValue = GetComponent<TMP_Text>();
        scoreValue.text = "0";
    }

    public void updateScore(int score)
    {
        scoreValue.text = score.ToString();
    }
}
