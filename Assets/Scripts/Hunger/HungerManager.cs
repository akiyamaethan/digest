using UnityEngine;

public class HungerManager : MonoBehaviour
{
    public static HungerManager instance;
    public float hungerLevel;

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
        HungerBar.instance.setHunger(hungerLevel);
    }

    public void setHunger(float amount)
    {
        hungerLevel = amount;
        HungerBar.instance.setHunger(hungerLevel);
    }

    public float getHunger()
    {
        return hungerLevel;
    }

}
