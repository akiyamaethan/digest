using UnityEngine;

public class AutoSwim : MonoBehaviour
{
    private int direction = 0;
    private float speed = 2f;
    [SerializeField] private int hpValue = 1;  // How much HP this fish gives
    [SerializeField] private float spawnPadding = 1f;  // How far off-screen to spawn

    private SpriteRenderer sr;
    private Camera cam;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    public void Initialize(string dir, float height)
    {
        if (cam == null) cam = Camera.main;
        Vector2 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

        Vector3 spawnPos = transform.position;
        spawnPos.y = height;

        if (dir == "left")
        {
            // Swimming left: start on RIGHT side, face left
            direction = -1;
            spawnPos.x = screenRight.x + spawnPadding;
            if (sr != null) sr.flipX = true;
        }
        else if (dir == "right")
        {
            // Swimming right: start on LEFT side, face right
            direction = 1;
            spawnPos.x = screenLeft.x - spawnPadding;
            if (sr != null) sr.flipX = false;
        }

        transform.position = spawnPos;
    }

    // Alias for backward compatibility
    public void initalize(string dir, float height) => Initialize(dir, height);

    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += direction * speed * Time.fixedDeltaTime;
        transform.position = currentPos;

        if (cam == null) cam = Camera.main;
        if (cam != null)
        {
            Vector2 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector2 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

            if ((direction == -1 && currentPos.x < screenLeft.x - spawnPadding) ||
                (direction == 1 && currentPos.x > screenRight.x + spawnPadding))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PointPlayerMovement player = collision.GetComponent<PointPlayerMovement>();
        if (player != null)
        {
            player.Heal(hpValue);
            Destroy(gameObject);
        }
    }
}

