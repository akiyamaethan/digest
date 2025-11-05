
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
    private SpriteRenderer playerSprite;

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
    

    private FishFollowMouse _player;
    private TMP_Text _gameOver;
    private TMP_Text _youStarved;
    private TMP_Text _youGotCaught;
    private GameObject _restart;
    public void initialize(FishFollowMouse player, TMP_Text gameOver, TMP_Text youStarved, TMP_Text youGotCaught, GameObject restartButton)
    {
        _player = player;
        playerSprite = _player.GetComponent<SpriteRenderer>();
        _gameOver = gameOver;
        _restart = restartButton;
        _youStarved = youStarved;
        _youGotCaught = youGotCaught;
        float adjustment = UnityEngine.Random.Range(-2f, 3f);
        pivotPoint.x += adjustment;
        pivotPoint.y += adjustment;
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

        if (!(caughtFish || baitEaten))
            finalAngle = totalAngle;
        if (caughtFish)
            totalAngle = finalAngle;

        if (justSpawned)
        {
            spawnTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(spawnTimer / spawnDuration);
            float dropCurve = Mathf.Sin(t * Mathf.PI);


            if (t < 0.5)
            {
                ropeLength = Mathf.Lerp(initialRopeLength, targetRopeLength + overshootRopeLength, t * 2f);
            }
            else
            {
                if (!spawnSoundPlayed)
                {
                    spawnSoundPlayed = true;
                    int soundToPlay = Random.Range(0, 2);
                    Debug.Log(soundToPlay);
                    if (soundToPlay == 1)
                        SoundManager.PlaySound(SoundName.SPLASH);
                    else
                        SoundManager.PlaySound(SoundName.SPLASH2);
                }
                ropeLength = Mathf.Lerp(targetRopeLength + overshootRopeLength, targetRopeLength, (float)(t - 0.5) * 2f);
            }

            if (t >= 1f)
            {
                justSpawned = false;
                ropeLength = 20f;
            }

        }

        Vector2 offset = new Vector2(Mathf.Sin(totalAngle * Mathf.Deg2Rad), -Mathf.Cos(totalAngle * Mathf.Deg2Rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;
        //immune/bobbing adjustment
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
                    gameOver();
                }
            }
            if (baitEaten)
            {
                baitReelTimer -= Time.fixedDeltaTime;
                if (baitReelTimer <= 0f)
                {
                    baitEaten = false;
                    HookManagerScript.instance.spawnNextHook();
                    Destroy(gameObject);
                }
            }
        }
        transform.position = pos;
        if (!(caughtFish || baitEaten))
            transform.rotation = Quaternion.Euler(0f, 0f, totalAngle);
    }


    private void gameOver()
    {
        Time.timeScale = 0;
        _gameOver.gameObject.SetActive(true);
        if (caughtFish)
            _youGotCaught.gameObject.SetActive(true);
        else
            _youStarved.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (baitEaten || caughtFish) 
            return;
        if (immunityTimer <= 0f)
        {
            _player.HP -= 1;
            HPManager.instance.updateHP(_player.HP);
            Debug.Log("HP: "+_player.HP);
            if (_player.HP <= 0)
            {
                _player.inputDisabled = true;
                caughtFish = true;
                Debug.Log("caught");
            }
            else
            {
                SoundManager.PlaySound(SoundName.SUS);
                immunityTimer = immunityDuration;
                StartCoroutine(BlinkDuringImmunity());
                HPManager.instance.blink();
            }
        }
    }

    private System.Collections.IEnumerator BlinkDuringImmunity()
    {
        float elapsed = 0f;
        bool visible = true;


        while (elapsed < immunityDuration)
        {
            visible = !visible;
            if (playerSprite != null)
                playerSprite.enabled = visible;

            yield return new WaitForSeconds(0.1f); // blink speed
            elapsed += 0.1f;
        }

        // make sure sprite is visible again
        if (playerSprite != null)
            playerSprite.enabled = true;

        immunityTimer = 0f; // end immunity
    }

    public void OnBaitEaten()
    {
        baitEaten = true;
        SoundManager.PlaySound(SoundName.HUH);
        Debug.Log("bait gobbled");
    }

}