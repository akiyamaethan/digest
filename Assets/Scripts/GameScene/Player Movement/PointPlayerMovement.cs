using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PointPlayerMovement : MonoBehaviour
{
    private const float DISABLED_VELOCITY = 2f;
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 20f;
    [SerializeField] public float inputDeadZone = 1.6f;
    public bool inputDisabled = false;
    public int HP = 3;
    private Vector2 direction = Vector2.zero;
    private float distance = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - rb.position).normalized;
        distance = Vector2.Distance(rb.position, mousePos);
    }

    void FixedUpdate()
    {
        if (inputDisabled)
        {
            rb.linearVelocity = Vector2.up * DISABLED_VELOCITY;
            return;
        }

        sr.flipY = direction.x < 0;

        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime));

        //Deadzone for mouse, fish wont move if mouse is on fish
        if (distance < inputDeadZone)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}