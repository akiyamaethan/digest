
using UnityEngine;

public class HookSwing : MonoBehaviour
{
    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private int hits;
    private bool caught = false;
    private Rigidbody2D rb;


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

    void Start()
    {
        randomOffset = Random.Range(0f, 100f);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float baseAngle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

  
        float noise = (Mathf.PerlinNoise(Time.time * noiseSpeed, randomOffset) - 0.5f) * noiseStrength;

        float totalAngle = baseAngle + noise;

   
        float rad = totalAngle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)) * ropeLength;
        Vector2 pos = pivotPoint + offset;

        if (bobTimer > 0f)
        {
            bobTimer -= Time.deltaTime;
            float bobOffsetY = Mathf.Sin((bobDuration - bobTimer) * Mathf.PI * 2f / bobDuration) * bobStrength;
            pos.y += bobOffsetY;
        }


        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle + 90);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        bobTimer = bobDuration;
        Debug.Log("hit: " + hits);
        hits++;

    }

}