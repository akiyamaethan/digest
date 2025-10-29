using UnityEngine;
using UnityEngine.UIElements;

public class Hook : MonoBehaviour
{
    private Ate food;
    private int hits;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hits++;
        Debug.Log("You die" + hits);
    }


}
