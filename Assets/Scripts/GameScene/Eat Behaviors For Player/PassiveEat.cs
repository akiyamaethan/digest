using UnityEngine;

public class PassiveEat : MonoBehaviour
{
    private Ate food;
    private Animator animator;
    private PointPlayerMovement playerMovement;
    private Collider2D playerCollider;

    private static readonly int IsEatingHash = Animator.StringToHash("isEating");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PointPlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerMovement != null && playerMovement.inputDisabled) return;
        food = collision.gameObject.GetComponent<Ate>();
        if (animator != null)
            animator.SetBool(IsEatingHash, true);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (playerMovement != null && playerMovement.inputDisabled) return;
        food = collision.gameObject.GetComponent<Ate>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        food = null;
        if (animator != null)
            animator.SetBool(IsEatingHash, false);
    }

    private void Update()
    {
        if (playerMovement != null && playerMovement.inputDisabled)
        {
            if (animator != null)
                animator.SetBool(IsEatingHash, false);
            return;
        }

        if (food != null && playerCollider != null)
        {
            food.Cut(playerCollider);
        }
    }
}