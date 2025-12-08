using UnityEngine;

/// <summary>
/// Manages player hunger level with passive drain over time.
/// Listens to hunger events and fires onHungerDepleted when starving.
/// No singleton access required - all communication via events.
/// </summary>
public class HungerManager : MonoBehaviour
{
    public HungerBar hungerBar;
    private float hungerLevel;
    private float hungerDrain = 0.3f;
    private float hungerDrainInterval = .05f;
    private float hungerDrainTimer = 0;
    private bool hasFiredDepleted = false;

    void Awake()
    {
        GameEvents.onHungerGain += AlterHunger;
        GameEvents.onHungerSet += SetHunger;
    }

    void OnDestroy()
    {
        GameEvents.onHungerGain -= AlterHunger;
        GameEvents.onHungerSet -= SetHunger;
    }

    void FixedUpdate()
    {
        if (hungerDrainTimer >= hungerDrainInterval)
        {
            hungerDrainTimer = 0f;
            AlterHunger(-hungerDrain);
        }
        else
        {
            hungerDrainTimer += Time.fixedDeltaTime;
        }
    }

    private void AlterHunger(float amount)
    {
        float previousHunger = hungerLevel;

        if (amount > 0)
        {
            if (hungerLevel <= 100f)
            {
                hungerLevel += amount;
                hasFiredDepleted = false; // Reset flag when hunger restored
            }
        }
        else
        {
            if (hungerLevel >= 0f)
                hungerLevel += amount;
        }

        // Clamp hunger level
        hungerLevel = Mathf.Clamp(hungerLevel, 0f, 100f);

        // Update UI
        if (hungerBar != null)
            hungerBar.setHunger(hungerLevel);

        // Fire depleted event when hunger hits 0 (only once per depletion)
        if (hungerLevel <= 0f && !hasFiredDepleted)
        {
            hasFiredDepleted = true;
            GameEvents.OnHungerDepleted();
        }
    }

    private void SetHunger(float amount)
    {
        hungerLevel = Mathf.Clamp(amount, 0f, 100f);
        hasFiredDepleted = false;
        if (hungerBar != null)
            hungerBar.setHunger(hungerLevel);
    }
}
