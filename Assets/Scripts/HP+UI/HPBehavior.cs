using TMPro;
using UnityEngine;

public class HPBehavior : MonoBehaviour
{
    public TMP_Text hpValue;
    public static HPBehavior instance;
    private float blinkDuration = 1.5f;


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

    public void blink()
    {
        StartCoroutine(blinkHP());
    }

    private System.Collections.IEnumerator blinkHP()
    {
        float elapsed = 0f;
        bool visible = true;

        hpValue.color = Color.red;
        while (elapsed < blinkDuration)
        {
            visible = !visible;
            if (hpValue != null)
                hpValue.enabled = visible;

            yield return new WaitForSeconds(0.1f); 
            elapsed += 0.1f;
        }

        if (hpValue != null)
            hpValue.enabled = true;
        hpValue.color = Color.white;
    }
}
