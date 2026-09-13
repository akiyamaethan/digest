using TMPro;
using UnityEngine;
using System.Collections;

// UI component that displays player HP with blink effects.
public class HPBehavior : MonoBehaviour
{
    private TMP_Text hpValue;
    private float blinkDuration = 1.5f;
    void Awake()
    {
        hpValue = GetComponent<TMP_Text>();
        GameEvents.onHPChanged += HandleHPChanged;
    }

    void OnDestroy()
    {
        GameEvents.onHPChanged -= HandleHPChanged;
    }

    private void HandleHPChanged(int newHP, int delta)
    {
        if (hpValue != null)
            hpValue.text = newHP.ToString();

        // If HP decreased (damage taken), trigger the red blink effect
        if (delta < 0)
        {
            Blink();
        }
    }

    private void Blink()
    {
        StartCoroutine(BlinkHP());
    }

    private IEnumerator BlinkHP()
    {
        float elapsed = 0f;
        bool visible = true;

        if (hpValue != null)
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
        {
            hpValue.enabled = true;
            hpValue.color = Color.white;
        }
    }
}
