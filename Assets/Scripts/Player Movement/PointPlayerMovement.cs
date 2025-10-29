using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FishFollowMouse : MonoBehaviour
{
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 20f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Convert mouse position to world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Direction toward the mouse
        Vector2 direction = (mousePos - rb.position).normalized;

        // Apply velocity
        rb.linearVelocity = direction * speed;

        // Smoothly rotate to face the direction of movement
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime));
        float distance = Vector2.Distance(rb.position, mousePos);
        if (distance < .9f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}