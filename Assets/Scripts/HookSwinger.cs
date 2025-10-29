
using UnityEngine;

public class HookSwing : MonoBehaviour
{
    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private int hits;
    private bool caughtFish = false;
    private float caughtHookOffsetY = 0f;



    [Header("Swing Settings")]
    [SerializeField] public float ropeLength = 20f;
    [SerializeField] public float swingSpeed = 0.2f;
    [SerializeField] public float swingAngle = 10f;
    [SerializeField] public float noiseSpeed = 0.2f;
    [SerializeField] public float noiseStrength = 14f;
    private float randomOffset;

    [Header("Bob Settings")]
    [SerializeField] private float bobDuration = 20f;
    [SerializeField] private float bobStrength = 2f;
    private float bobTimer = 0f;

    [Header("Player Settings")]
    [SerializeField] private FishFollowMouse player;

    void Start()
    {
        randomOffset = Random.Range(0f, 100f);

    }

    void Update()
    {
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

  
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;

        float totalAngle = baseAngle + noise;

   
        float rad = totalAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;

        if (bobTimer > 0f && !caughtFish)
        {
            bobTimer -= Time.deltaTime;
            float bobOffsetY = Mathf.Sin((bobDuration - bobTimer) * Mathf.PI * 2f / bobDuration) * bobStrength;
            pos.y += bobOffsetY;
        }

        if (caughtFish)
        {
            caughtHookOffsetY += .08f;
            pos.y += caughtHookOffsetY;
        }
        Debug.Log("pos: " + pos);
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle + 90);


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bobTimer = bobDuration;
        Debug.Log("hit: " + hits);
        hits++;
        if (hits >= 3)
        {
            player.inputDisabled = true;
            caughtFish = true;
        }

    }

}