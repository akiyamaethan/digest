using TMPro;
using UnityEngine;

public class HPBehavior : SingletonNoPersist<HPBehavior>
{
    public TMP_Text hpValue;
    private float blinkDuration = 1.5f;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
    }

    void Start()
    {
        hpValue = GetComponent<TMP_Text>();
        hpValue.text = "0";
    }

    public void updateHP(int hp)
    {
        hpValue.text = hp.ToString();
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
