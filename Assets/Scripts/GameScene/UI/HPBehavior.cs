using TMPro;
using UnityEngine;

public class HPBehavior : SingletonNoPersist<HPBehavior>
{
    public TMP_Text hpValue;
    private float blinkDuration = 1.5f;
    private int currentHP = 0;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
    }

    void Start()
    {
        hpValue = GetComponent<TMP_Text>();
        hpValue.text = "0";
    }

    void OnEnable()
    {
        // Subscribe to HP events
        GameEvents.onHPGain += HandleHPGain;
        GameEvents.onHPLoss += HandleHPLoss;
        GameEvents.onHPChange += HandleHPChange;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        GameEvents.onHPGain -= HandleHPGain;
        GameEvents.onHPLoss -= HandleHPLoss;
        GameEvents.onHPChange -= HandleHPChange;
    }

    private void HandleHPGain(int amount)
    {
        currentHP += amount;
        UpdateDisplay();
        // Could add a heal blink effect here if desired
    }

    private void HandleHPLoss(int amount)
    {
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        UpdateDisplay();
        blink();  // Blink on damage
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

    public void blink()
    {
        StartCoroutine(blinkHP());
    }

    private System.Collections.IEnumerator blinkHP()
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
