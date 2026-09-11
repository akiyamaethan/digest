using UnityEngine;
using System.Collections;

/// Controls individual hook behavior - swinging, spawning animation, and player collision.
public class HookSwing : MonoBehaviour
{
    [SerializeField] private GameObject cautionUI;

    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private bool caughtFish = false;
    private float caughtHookOffsetY = 0f;
    public bool baitEaten = false;

    private float caughtFishTimer = 4f;
    private float baitReelTimer = 4f;
    private float immunityTimer = 0f;
    private float immunityDuration = 1.5f;
    private SpriteRenderer playerSprite;

    private static readonly WaitForSeconds BlinkWait = new WaitForSeconds(0.1f);
    private static Canvas cachedCanvas;

    [Header("Spawn Settings")]
    private bool justSpawned = true;
    private float spawnTimer = 0f;
    private float spawnDuration = 2f;
    public float initialRopeLength = 0f;
    public float overshootRopeLength = 1f;
    public float targetRopeLength = 20f;
    private bool spawnSoundPlayed = false;
    private float finalAngle = 0f;

    [Header("Swing Settings")]
    public float ropeLength = 20f;

    public float swingSpeed = 0.4f;
    public float swingAngle = 10f;
    public float noiseSpeed = 0.2f;
    public float noiseStrength = 14f;
    private float randomOffset;

    [Header("Bob Settings")]
    private float bobStrength = .5f;

    void Awake()
    {
        randomOffset = Random.Range(0f, 100f);
        ropeLength = initialRopeLength;

        float adjustmentX = Random.Range(-4f, 5f);
        float adjustmentY = Random.Range(-2f, 3f);
        pivotPoint.x += adjustmentX;
        pivotPoint.y += adjustmentY;
    }

    void Start()
    {
        PositionAndWarn();
    }

    private void PositionAndWarn()
    {
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float totalAngle = baseAngle + noise;
        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle);

        if (cautionUI != null)
        {
            if (cachedCanvas == null)
                cachedCanvas = FindFirstObjectByType<Canvas>();

            if (cachedCanvas != null)
            {
                GameObject caution = Instantiate(cautionUI, cachedCanvas.transform);
                WarningController cautionScript = caution.GetComponent<WarningController>();
                if (cautionScript != null)
                {
                    cautionScript.Initialize(transform);
                }
            }
        }
    }

    void FixedUpdate()
    {
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float totalAngle = baseAngle + noise;

        if (!(caughtFish || baitEaten))
            finalAngle = totalAngle;
        if (caughtFish)
            totalAngle = finalAngle;

        if (justSpawned)
        {
            spawnTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(spawnTimer / spawnDuration);

            if (t < 0.5f)
            {
                ropeLength = Mathf.Lerp(initialRopeLength, targetRopeLength + overshootRopeLength, t * 2f);
            }
            else
            {
                if (!spawnSoundPlayed)
                {
                    spawnSoundPlayed = true;
                    int soundToPlay = Random.Range(0, 2);
                    if (soundToPlay == 1)
                        GameEvents.OnPlaySound(SoundName.SPLASH);
                    else
                        GameEvents.OnPlaySound(SoundName.SPLASH2);
                }
                ropeLength = Mathf.Lerp(targetRopeLength + overshootRopeLength, targetRopeLength, (t - 0.5f) * 2f);
            }

            if (t >= 1f)
            {
                justSpawned = false;
                ropeLength = 20f;
            }
        }

        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;

        // Immune/bobbing adjustment
        if (immunityTimer > 0f)
        {
            immunityTimer -= Time.fixedDeltaTime;
            float bobOffsetY = Mathf.Sin((immunityDuration - immunityTimer) * Mathf.PI * 2f / immunityDuration) * bobStrength;
            pos.y += bobOffsetY;
        }

        if (caughtFish || baitEaten)
        {
            caughtHookOffsetY += .03f;
            pos.y += caughtHookOffsetY;
            if (caughtFish)
            {
                caughtFishTimer -= Time.fixedDeltaTime;
                if (caughtFishTimer <= 0f)
                {
                    GameEvents.OnCheckHighScore();
                    GameEvents.OnGameOver(GameOverCause.Caught);
                }
            }
            if (baitEaten)
            {
                baitReelTimer -= Time.fixedDeltaTime;
                if (baitReelTimer <= 0f)
                {
                    baitEaten = false;
                    GameEvents.OnSpawnNextHookRequested();
                    Destroy(gameObject);
                }
            }
        }
        transform.position = pos;
        if (!(caughtFish || baitEaten))
            transform.rotation = Quaternion.Euler(0f, 0f, totalAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (baitEaten || caughtFish)
            return;

        PointPlayerMovement player = collision.GetComponent<PointPlayerMovement>();
        if (player == null)
            return;

        playerSprite = player.GetComponent<SpriteRenderer>();

        if (immunityTimer <= 0f)
        {
            player.TakeDamage(1);
            if (player.HP <= 0)
            {
                player.inputDisabled = true;
                caughtFish = true;
                Debug.Log("caught");
            }
            else
            {
                int soundToPlay = Random.Range(0, 2);
                if (soundToPlay == 1)
                    GameEvents.OnPlaySound(SoundName.HOOK1, .5f);
                else
                    GameEvents.OnPlaySound(SoundName.HOOK2, .5f);
                immunityTimer = immunityDuration;
                StartCoroutine(BlinkDuringImmunity());
            }
        }
    }

    private IEnumerator BlinkDuringImmunity()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < immunityDuration)
        {
            visible = !visible;
            if (playerSprite != null)
                playerSprite.enabled = visible;

            yield return BlinkWait;
            elapsed += 0.1f;
        }

        if (playerSprite != null)
            playerSprite.enabled = true;

        immunityTimer = 0f;
    }

    public void OnBaitEaten()
    {
        baitEaten = true;
        int soundToPlay = Random.Range(0, 2);
        if (soundToPlay == 1)
            GameEvents.OnPlaySound(SoundName.GRUNT1);
        else
            GameEvents.OnPlaySound(SoundName.GRUNT2);
    }
}