using UnityEngine;
using UnityEngine.UIElements;

public class Eat : MonoBehaviour
{
    private Ate food;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        food = collision.gameObject.GetComponent<Ate>();
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        food = collision.gameObject.GetComponent<Ate>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        food = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
                       if (food != null)
            {
                food.Cut(GetComponent<Collider2D>());
            }
        }
    }

}
