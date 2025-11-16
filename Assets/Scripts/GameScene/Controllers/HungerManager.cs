using UnityEngine;

public class HungerManager : MonoBehaviour
{
    public HungerBar hungerBar;
    public static HungerManager instance;
    public float hungerLevel;
    private float hungerDrain = 0.3f;
    private float hungerDrainInterval = .05f;
    private float hungerDrainTimer = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GameEvents.onHungerGain += alterHunger;
    }

    private void OnDestroy()
    {
        GameEvents.onHungerGain -= alterHunger;
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
