using UnityEngine;

public enum SwimDirection
{
    Left,
    Right
}

public class AutoSwim : MonoBehaviour
{
    private int direction = 0;
    private float speed = 2f;
    [SerializeField] private int hpValue = 1;  // How much HP this fish gives
    [SerializeField] private float spawnPadding = 1f;  // How far off-screen to spawn

    private SpriteRenderer sr;
    private Camera cam;
    private float destroyBoundaryX;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    public void Initialize(SwimDirection dir, float height)
    {
        if (cam == null) cam = Camera.main;
        if (cam != null)
        {
            Vector2 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector2 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

            Vector3 spawnPos = transform.position;
            spawnPos.y = height;

            if (dir == SwimDirection.Left)
            {
                // Swimming left: start on RIGHT side, face left
                direction = -1;
                spawnPos.x = screenRight.x + spawnPadding;
                destroyBoundaryX = screenLeft.x - spawnPadding;
                if (sr != null) sr.flipX = true;
            }
            else
            {
                // Swimming right: start on LEFT side, face right
                direction = 1;
                spawnPos.x = screenLeft.x - spawnPadding;
                destroyBoundaryX = screenRight.x + spawnPadding;
                if (sr != null) sr.flipX = false;
            }

            transform.position = spawnPos;
        }
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += direction * speed * Time.deltaTime;
        transform.position = currentPos;

        if ((direction == -1 && currentPos.x < destroyBoundaryX) ||
            (direction == 1 && currentPos.x > destroyBoundaryX))
        {
            Destroy(gameObject);
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

