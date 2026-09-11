using UnityEngine;
using System.Collections.Generic;

public class PassiveEat : MonoBehaviour
{
    private readonly List<Ate> foodsInRange = new List<Ate>();
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

        Ate food = collision.GetComponent<Ate>();
        if (food != null && !foodsInRange.Contains(food))
        {
            foodsInRange.Add(food);
            if (animator != null)
                animator.SetBool(IsEatingHash, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ate food = collision.GetComponent<Ate>();
        if (food != null)
        {
            foodsInRange.Remove(food);
        }

        if (foodsInRange.Count == 0 && animator != null)
        {
            animator.SetBool(IsEatingHash, false);
        }
    }

    private void Update()
    {
        if (playerMovement != null && playerMovement.inputDisabled)
        {
            if (animator != null)
                animator.SetBool(IsEatingHash, false);
            foodsInRange.Clear();
            return;
        }

        if (playerCollider == null) return;

        for (int i = foodsInRange.Count - 1; i >= 0; i--)
        {
            Ate food = foodsInRange[i];
            if (food == null || !food.gameObject.activeInHierarchy)
            {
                foodsInRange.RemoveAt(i);
                continue;
            }

            food.Cut(playerCollider);
        }

        if (animator != null)
        {
            animator.SetBool(IsEatingHash, foodsInRange.Count > 0);
        }
    }
}