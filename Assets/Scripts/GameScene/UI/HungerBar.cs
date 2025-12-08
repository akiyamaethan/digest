using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    public Slider slider;

    public void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setHunger(float arg)
    {
        if (slider != null)
            slider.value = arg;
    }

}
