using UnityEngine;
using TMPro;

public class FlavorText : MonoBehaviour
{
    private TMP_Text flavorText;
    void Start()
    {
        flavorText = GetComponent<TMP_Text>();
        setFlavorText();
    }

    void setFlavorText()
    {
        int choice = Random.Range(0, 6);
        if (choice == 0)
        {
            flavorText.text = "Flavor Text";
        }
        else if (choice == 1)
        {
            flavorText.text = "Catch me if you can!";
        }
        else if (choice == 2)
        {
            flavorText.text = "Have we met before?";
        }
        else if (choice == 3)
        {
            flavorText.text = "I'm lonely";
        }
        else if (choice == 4)
        {
            flavorText.text = "What are you looking at?";
        }
        else if (choice == 5)
        {
            flavorText.text = "Why did the fish cross the ocean?";
        }
    }
}
