using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PointPlayerMovement : MonoBehaviour
{
    private const float DISABLED_VELOCITY = 1.5f;
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 20f;
    [SerializeField] public float inputDeadZone = 1.6f;
    [SerializeField] public float boundsPadding = 0.5f;
    public bool inputDisabled = false;
    public int HP = 3;
    private Vector2 direction = Vector2.zero;
    private float distance = 0f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Camera mainCam;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        mainCam = Camera.main;
        CalculateBounds();
    }

    void CalculateBounds()
    {
        // Get camera viewport corners in world space
        Vector2 bottomLeft = mainCam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Apply padding so fish stays fully on screen
        minBounds = bottomLeft + Vector2.one * boundsPadding;
        maxBounds = topRight - Vector2.one * boundsPadding;
    }

    void OnEnable()
    {
        GameEvents.onHPGain += HandleHPGain;
    }

    void OnDisable()
    {
        GameEvents.onHPGain -= HandleHPGain;
    }

    private void HandleHPGain(int amount)
    {
        HP += amount;
        Debug.Log("[Player] HP gained! Now at: " + HP);
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

    void LateUpdate()
    {
        // Skip clamping when input is disabled (game over) so fish can be reeled off screen
        if (inputDisabled) return;

        // Clamp position to stay within screen bounds
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minBounds.x, maxBounds.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minBounds.y, maxBounds.y);
        transform.position = clampedPos;
    }
}