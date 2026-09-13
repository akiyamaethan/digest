using UnityEngine;

// Smoothly pans the background in the opposite direction of the player's movement/position

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float parallaxFactor = 0.08f;
    [SerializeField] private float maxOffset = 1.0f;
    [SerializeField] private float smoothSpeed = 3.0f;

    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            PointPlayerMovement player = FindFirstObjectByType<PointPlayerMovement>();
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        // Calculate offset in the opposite direction of the player's world position
        Vector3 targetOffset = -playerTransform.position * parallaxFactor;

        // Clamp offset so the background never shifts off-screen
        targetOffset.x = Mathf.Clamp(targetOffset.x, -maxOffset, maxOffset);
        targetOffset.y = Mathf.Clamp(targetOffset.y, -maxOffset, maxOffset);
        targetOffset.z = 0f;

        Vector3 targetPosition = initialPosition + targetOffset;

        // Smoothly interpolate towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
