using UnityEngine;

public class HungerManager : MonoBehaviour
{
    public static HungerManager instance;

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
        HungerBar.instance.alterHunger(amount);
    }

    public void setHunger(float amount)
    {
        HungerBar.instance.setHunger(amount);
    }

}
