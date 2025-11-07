using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;
    public static HungerBar instance;

    private void Awake()
    {
        instance = this;
        slider = GetComponent<Slider>();
    }
    public void setHunger(float arg)
    {
        slider.value = arg;
    }

}
