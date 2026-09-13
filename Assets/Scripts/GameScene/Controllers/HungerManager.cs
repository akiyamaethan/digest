using UnityEngine;

// Manages player hunger level with passive drain over time.
// Listens to hunger events and triggers GameOver (Starved) when hunger is depleted.
public class HungerManager : MonoBehaviour
{
    private float hungerLevel;
    private float maxHunger = 100f;
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
        if (amount > 0)
        {
            if (hungerLevel <= maxHunger)
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
        hungerLevel = Mathf.Clamp(hungerLevel, 0f, maxHunger);

        // Notify UI and listeners via event bus
        GameEvents.OnHungerUpdated(hungerLevel);

        // Trigger game over when hunger hits 0 (only once per depletion)
        if (hungerLevel <= 0f && !hasFiredDepleted)
        {
            hasFiredDepleted = true;
            GameEvents.OnGameOver(GameOverCause.Starved);
        }
    }

    private void SetHunger(float amount)
    {
        hungerLevel = Mathf.Clamp(amount, 0f, maxHunger);
        hasFiredDepleted = false;
        GameEvents.OnHungerUpdated(hungerLevel);
    }
}
