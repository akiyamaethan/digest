
using UnityEngine;
using TMPro;
using System.Collections;


public class HookSwing : MonoBehaviour
{
    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private bool caughtFish = false;
    private float caughtHookOffsetY = 0f;
    public bool baitEaten = false;
    
    private float caughtFishTimer = 4f;
    private float baitReelTimer = 4f;
    private float reelSpeed = 10f;

    [Header("Swing Settings")]
    public float ropeLength = 20f;
    public float swingSpeed = 0.2f;
    public float swingAngle = 10f;
    public float noiseSpeed = 0.2f;
    public float noiseStrength = 14f;
    private float randomOffset;

    [Header("Bob Settings")]
    private float bobDuration = 20f;
    private float bobStrength = 2f;
    private float bobTimer = 0f;

    private FishFollowMouse _player;
    private TMP_Text _gameOver;
    public void initialize(FishFollowMouse player, TMP_Text gameOver)
    {
        _player = player;
        _gameOver = gameOver;
        float adjustment = UnityEngine.Random.Range(-1f, 1f);
        pivotPoint.x += adjustment;
    }

    void Start()
    {
        randomOffset = Random.Range(0f, 100f);

    }

    void FixedUpdate()
    {
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float totalAngle = baseAngle + noise;

        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;

        // bobbing
        if (bobTimer > 0f && !caughtFish && !baitEaten)
        {
            bobTimer -= Time.fixedDeltaTime;
            float bobOffsetY = Mathf.Sin((bobDuration - bobTimer) * Mathf.PI * 2f / bobDuration) * bobStrength;
            pos.y += bobOffsetY;
        }

        if (caughtFish || baitEaten)
        {
            caughtHookOffsetY += .03f;
            if (caughtFish)
            {
                pos.y += caughtHookOffsetY;
                caughtFishTimer -= Time.fixedDeltaTime;
                if (caughtFishTimer <= 0f)
                {
                    caughtFish = false;
                    gameOver();
                }
            }

            // baitEaten motion
            if (baitEaten)
            {
                pos.y += caughtHookOffsetY;
                baitReelTimer -= Time.fixedDeltaTime;
                if (baitReelTimer <= 0f)
                {
                    baitEaten = false;
                    HookManagerScript.instance.spawnNewHook();
                    Destroy(gameObject);
                }
            }
        }
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle);
    }


    private void gameOver()
    {
        Time.timeScale = 0;
        _gameOver.gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bobTimer = bobDuration;
        _player.HP -= 1;
        
        if (_player.HP <= 0)
        {
            _player.inputDisabled = true;
            caughtFish = true;
        }
        
    }

    public void OnBaitEaten()
    {
        baitEaten = true;
    }

}