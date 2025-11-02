
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class HookSwing : MonoBehaviour
{
    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private bool caughtFish = false;
    private float caughtHookOffsetY = 0f;
    public bool baitEaten = false;
    
    private float caughtFishTimer = 4f;
    private float baitReelTimer = 4f;
    private float immunityTimer = 0f;
    private float immunityDuration = 1.5f;

    [Header("Spawn Settings")]
    private bool justSpawned = true;
    private float spawnTimer = 0f;
    private float spawnDuration = 2f;
    public float initialRopeLength = 0f;
    public float overshootRopeLength = 1f;
    public float targetRopeLength = 20f;

    [Header("Swing Settings")]
    public float ropeLength = 20f;
    
    public float swingSpeed = 0.4f;
    public float swingAngle = 10f;
    public float noiseSpeed = 0.2f;
    public float noiseStrength = 14f;
    private float randomOffset;

    [Header("Bob Settings")]
    private float bobDuration = 5f;
    private float bobStrength = 1f;
    private float bobTimer = 0f;

    private FishFollowMouse _player;
    private TMP_Text _gameOver;
    private GameObject _restart;
    public void initialize(FishFollowMouse player, TMP_Text gameOver, GameObject restartButton)
    {
        _player = player;
        _gameOver = gameOver;
        _restart = restartButton;
        float adjustment = UnityEngine.Random.Range(-2f, 2f);
        pivotPoint.x += adjustment;
    }

    void Awake()
    {
        randomOffset = Random.Range(0f, 100f);
        ropeLength = initialRopeLength;
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float totalAngle = baseAngle + noise;
        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle);
    }

    void FixedUpdate()
    {
        if (HungerManager.instance.getHunger() <= 0f)
            gameOver();

        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;
        float totalAngle = baseAngle + noise;

        if (justSpawned)
        {
            spawnTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(spawnTimer / spawnDuration);
            float dropCurve = Mathf.Sin(t * Mathf.PI);
            

             if (t<0.5)
            {
                ropeLength = Mathf.Lerp(initialRopeLength, targetRopeLength + overshootRopeLength, t * 2f);
            }
            else
            {
                ropeLength = Mathf.Lerp(targetRopeLength + overshootRopeLength, targetRopeLength, (float)(t - 0.5) * 2f);
            }

            if (t >= 1f)
            {
                justSpawned = false;
                ropeLength = 20f;
            }

        }
        if (immunityTimer > 0f)
        {
            immunityTimer -= Time.fixedDeltaTime;
        }
      

        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;

        // bobbing
        if (!justSpawned && bobTimer > 0f && !caughtFish && !baitEaten)
        {
            bobTimer -= Time.fixedDeltaTime;
            float bobOffsetY = Mathf.Sin((bobDuration - bobTimer) * Mathf.PI * 2f / bobDuration) * bobStrength;
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
                    caughtFish = false;
                    gameOver();
                }
            }
            if (baitEaten)
            {
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
        _restart.gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (immunityTimer <= 0f)
        {
            _player.HP -= 1;
            Debug.Log("HP: "+_player.HP);
            immunityTimer = immunityDuration;

            if (_player.HP <= 0)
            {
                _player.inputDisabled = true;
                caughtFish = true;
                Debug.Log("caught");
            }
        }
        if (bobTimer == 0f) 
            bobTimer = bobDuration;   
    }

    public void OnBaitEaten()
    {
        baitEaten = true;
        Debug.Log("bait gobbled");
    }

}