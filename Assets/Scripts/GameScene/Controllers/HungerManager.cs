using UnityEngine;

public class HungerManager : SingletonNoPersist<HungerManager>
{
    public HungerBar hungerBar;
    public float hungerLevel;
    private float hungerDrain = 0.3f;
    private float hungerDrainInterval = .05f;
    private float hungerDrainTimer = 0;

    protected override void Awake()
    {
        base.Awake();  // Handle singleton logic
        GameEvents.onHungerGain += alterHunger;
    }

    protected override void OnDestroy()
    {
        GameEvents.onHungerGain -= alterHunger;
        base.OnDestroy();  // Clean up singleton reference
    }

    void FixedUpdate()
    {
        if (hungerDrainTimer >= hungerDrainInterval)
        {
            hungerDrainTimer = 0f;
            alterHunger(-hungerDrain);
        }
        else
        {
            hungerDrainTimer += Time.fixedDeltaTime;
        }

    }
    public void alterHunger(float amount)
    {
        if (amount > 0)
        {
            if (hungerLevel <= 100f)
            {
                hungerLevel += amount;
            }
        }
        else
            if (hungerLevel >= 0f)
                hungerLevel += amount;
        hungerBar.setHunger(hungerLevel);
    }

    public void setHunger(float amount)
    {
        hungerLevel = amount;
        hungerBar.setHunger(hungerLevel);
    }

    public float getHunger()
    {
        return hungerLevel;
    }

}
