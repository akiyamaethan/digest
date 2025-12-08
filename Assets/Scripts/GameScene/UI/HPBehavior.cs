using TMPro;
using UnityEngine;
using System.Collections;

/// <summary>
/// UI component that displays player HP with blink effects.
/// Subscribes to HP events - no singleton access required.
/// </summary>
public class HPBehavior : MonoBehaviour
{
    private TMP_Text hpValue;
    private float blinkDuration = 1.5f;
    private int currentHP = 0;

    void Awake()
    {
        GameEvents.onHPGain += HandleHPGain;
        GameEvents.onHPLoss += HandleHPLoss;
        GameEvents.onHPChange += HandleHPChange;
    }

    void OnDestroy()
    {
        GameEvents.onHPGain -= HandleHPGain;
        GameEvents.onHPLoss -= HandleHPLoss;
        GameEvents.onHPChange -= HandleHPChange;
    }

    void Start()
    {
        hpValue = GetComponent<TMP_Text>();
        hpValue.text = "0";
    }

    private void HandleHPGain(int amount)
    {
        currentHP += amount;
        UpdateDisplay();
    }

    private void HandleHPLoss(int amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateDisplay();
        Blink();
    }

    private void HandleHPChange(int newHP)
    {
        currentHP = newHP;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (hpValue != null)
            hpValue.text = currentHP.ToString();
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
