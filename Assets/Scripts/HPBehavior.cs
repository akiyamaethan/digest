using TMPro;
using UnityEngine;

public class HPBehavior : MonoBehaviour
{
    public TMP_Text hpValue;
    public static HPBehavior instance;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        hpValue = GetComponent<TMP_Text>();
        hpValue.text = "HP: 0";
    }

    public void updateHP(int hp)
    {
        hpValue.text = "HP: " + hp.ToString();
    }
}
