using UnityEngine;
using UnityEngine.UIElements;

public class PassiveEat : MonoBehaviour
{
    private Ate food;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        food = collision.gameObject.GetComponent<Ate>();
        animator.SetBool("isEating", true);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        food = collision.gameObject.GetComponent<Ate>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        food = null;
        animator.SetBool("isEating", false);
    }

    private void Update()
    {
        
            if (food != null)
            {
                food.Cut(GetComponent<Collider2D>());
            }
        
    }

}