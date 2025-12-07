
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;


public class HookSwing : MonoBehaviour
{
    public GameObject cautionUI;
    
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
    

    private PointPlayerMovement _player;
    private TMP_Text _gameOver;
    private TMP_Text _youStarved;
    private TMP_Text _youGotCaught;
    private GameObject _restart;
    private GameObject _title;
    private Canvas _mainCanvas;
    private GameObject _gameOverHighScore;

    public void initialize(HookInitializationData data)
    {
        _player = data.player;
        playerSprite = _player.GetComponent<SpriteRenderer>();
        _gameOver = data.gameOver;
        _restart = data.restartButton;
        _youStarved = data.youStarved;
        _youGotCaught = data.youGotCaught;
        _mainCanvas = data.mainCanvas;
        _title = data.title;
        _gameOverHighScore = data.gameOverHighScore;
        float adjustmentX = UnityEngine.Random.Range(-4f, 5f);
        float adjustmentY = UnityEngine.Random.Range(-2f, 3f);
        pivotPoint.x += adjustmentX;
        pivotPoint.y += adjustmentY;

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

            GameObject caution = Instantiate(cautionUI, _mainCanvas.transform);
            WarningController cautionScript = caution.GetComponent<WarningController>();
            cautionScript.initialize(transform);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
            Vector3 cautionPos = caution.transform.position;
            cautionPos.x = screenPos.x;

    }

    void Awake()
    {
        randomOffset = Random.Range(0f, 100f);
        ropeLength = initialRopeLength;
        
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
        // Check for new high score before setting game over state
        ScoreManager.CheckAndUpdateHighScore();

        // Use GameStateManager if available, otherwise fall back to direct Time.timeScale
        if (GameStateManager.instance != null)
        {
            GameStateManager.SetGameOver();
        }
        else
        {
            Time.timeScale = 0f;
        }

        _gameOver.gameObject.SetActive(true);
        if (caughtFish)
            _youGotCaught.gameObject.SetActive(true);
        else
            _youStarved.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
        _title.gameObject.SetActive(true);
        if (_gameOverHighScore != null)
            _gameOverHighScore.SetActive(true);
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
                SoundManager.PlaySound(SoundName.SUS, .5f);
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
    }

}