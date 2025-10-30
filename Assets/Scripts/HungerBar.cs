using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void incrementHunger(float arg)
    {
        slider.value += arg;
    }
    public void setHunger(float arg)
    {
        slider.value = arg;
    }

    public void decrementHunger(float arg)
    {
        slider.value -= arg;
    }
}
