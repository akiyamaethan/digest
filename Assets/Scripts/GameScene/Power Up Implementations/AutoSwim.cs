using UnityEngine;

public class AutoSwim : MonoBehaviour
{
    private int direction = 0;
    private float speed = 2f;
    [SerializeField] private int hpValue = 1;  // How much HP this fish gives
    [SerializeField] private float spawnPadding = 1f;  // How far off-screen to spawn

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void initalize(string dir, float height)
    {
        Camera cam = Camera.main;
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

    void FixedUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += direction * speed * Time.deltaTime;
        transform.position = currentPos;

        // Destroy if swam off the opposite side of screen
        Camera cam = Camera.main;
        Vector2 screenLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 screenRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

        if ((direction == -1 && currentPos.x < screenLeft.x - spawnPadding) ||
            (direction == 1 && currentPos.x > screenRight.x + spawnPadding))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fire the HP gain event - any subscriber will handle it
        GameEvents.OnHPGain(hpValue);
        Destroy(gameObject);
    }
}
