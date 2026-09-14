using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]

// Controls Player movement via mouse pointer as well as player HP
public class PointPlayerMovement : MonoBehaviour
{
    private const float DISABLED_VELOCITY = 1.5f;
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 20f;
    [SerializeField] public float inputDeadZone = 1.6f;
    [SerializeField] public float boundsPadding = 0.5f;
    public bool inputDisabled = false;
    public int HP = 3;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Camera mainCam;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Coroutine gameOverCoroutine;

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

    void Start()
    {
        // Broadcast starting HP to UI
        GameEvents.OnHPChanged(HP, 0);
    }

    public void TakeDamage(int amount)
    {
        if (HP <= 0) return;

        HP -= amount;
        if (HP < 0) HP = 0;
        GameEvents.OnHPChanged(HP, -amount);
        Debug.Log("[Player] Took damage! HP now: " + HP);

        if (HP <= 0 && gameOverCoroutine == null)
        {
            inputDisabled = true;
            gameOverCoroutine = StartCoroutine(CaughtGameOverSequence());
        }
    }

    private IEnumerator CaughtGameOverSequence()
    {
        yield return new WaitForSeconds(1.5f);
        GameEvents.OnGameOver(GameOverCause.Caught);
    }

    public void Heal(int amount)
    {
        if (HP <= 0) return;

        HP += amount;
        GameEvents.OnHPChanged(HP, amount);
        Debug.Log("[Player] Healed! HP now: " + HP);
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        if (inputDisabled)
        {
            rb.linearVelocity = Vector2.up * DISABLED_VELOCITY;
            return;
        }

        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toMouse = mousePos - rb.position;
        float distance = toMouse.magnitude;
        Vector2 direction = toMouse.normalized;
        sr.flipY = direction.x < 0;

        // Movement/deadzone
        if (distance < inputDeadZone)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = direction * speed;
        }

        // Smooth rotation
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime);

        // Keep fish on screen
        Vector2 clampedPos = rb.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minBounds.x, maxBounds.x);
        clampedPos.y = Mathf.Clamp(clampedPos.y, minBounds.y, maxBounds.y);
        rb.position = clampedPos;
    }
}