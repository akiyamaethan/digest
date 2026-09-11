using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        GameEvents.onHungerUpdated += SetHunger;
    }

    private void OnDestroy()
    {
        GameEvents.onHungerUpdated -= SetHunger;
    }

    public void SetHunger(float arg)
    {
        if (slider != null)
            slider.value = arg;
    }
}
