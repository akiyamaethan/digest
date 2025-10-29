
using UnityEngine;

public class HookSwing : MonoBehaviour
{
    public Vector2 pivotPoint = new Vector2(0f, 22f);
    private LineRenderer line;
    [SerializeField] public float ropeLength = 20f;
    [SerializeField] public float swingSpeed = 0.2f;
    [SerializeField] public float swingAngle = 10f;
    [SerializeField] public float noiseSpeed = 0.2f;
    [SerializeField] public float noiseStrength = 14f;

    private float randomOffset;

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

       
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0f, 0f, totalAngle + 90);

    }
}