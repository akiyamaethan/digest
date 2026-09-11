using UnityEngine;
using TMPro;

public class FlavorText : MonoBehaviour
{
    private static readonly string[] FlavorOptions =
    {
        "Flavor Text",
        "Catch me if you can!",
        "Have we met before?",
        "I'm lonely",
        "What are you looking at?",
        "Why did the fish cross the ocean?",
        "What a nice day to go fishing!",
        "Just keep swimming",
        "Feed me worms"
    };

    [SerializeField] private TMP_Text flavorText;

    private void Awake()
    {
        if (flavorText == null)
            flavorText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        SetFlavorText();
    }

    public void SetFlavorText()
    {
        if (flavorText != null && FlavorOptions.Length > 0)
        {
            flavorText.text = FlavorOptions[Random.Range(0, FlavorOptions.Length)];
        }
    }
}
