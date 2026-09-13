using UnityEngine;

// Smoothly shifts a UI RectTransform or world Transform in the opposite direction
// of the player's mouse cursor for a rich menu / screen parallax effect.
public class MouseParallax : MonoBehaviour
{
    [SerializeField] private Vector2 maxOffset = new Vector2(30f, 20f);
    [SerializeField] private float smoothSpeed = 4f;
    [SerializeField] private bool enableIdleSway = true;
    [SerializeField] private Vector2 idleSwayAmount = new Vector2(5f, 5f);
    [SerializeField] private float idleSwaySpeed = 1f;

    private RectTransform rectTransform;
    private Vector2 initialAnchoredPosition;
    private Vector3 initialWorldPosition;
    private bool isUIElement;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        isUIElement = rectTransform != null;

        if (isUIElement)
        {
            initialAnchoredPosition = rectTransform.anchoredPosition;
        }
        else
        {
            initialWorldPosition = transform.position;
        }
    }

    private void LateUpdate()
    {
        // Calculate normalized mouse position from screen center (-1 to +1)
        Vector2 mousePos = Input.mousePosition;
        float normX = Mathf.Clamp((mousePos.x / Screen.width - 0.5f) * 2f, -1f, 1f);
        float normY = Mathf.Clamp((mousePos.y / Screen.height - 0.5f) * 2f, -1f, 1f);

        // Invert direction so background moves opposite to mouse
        Vector2 targetOffset = new Vector2(-normX * maxOffset.x, -normY * maxOffset.y);

        // Optional gentle idle sway
        if (enableIdleSway)
        {
            float swayX = Mathf.Sin(Time.unscaledTime * idleSwaySpeed) * idleSwayAmount.x;
            float swayY = Mathf.Cos(Time.unscaledTime * idleSwaySpeed * 0.8f) * idleSwayAmount.y;
            targetOffset += new Vector2(swayX, swayY);
        }

        // Apply smooth interpolation using unscaledDeltaTime (works in menus/paused state)
        if (isUIElement)
        {
            Vector2 targetPos = initialAnchoredPosition + targetOffset;
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, smoothSpeed * Time.unscaledDeltaTime);
        }
        else
        {
            Vector3 targetPos = initialWorldPosition + (Vector3)targetOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.unscaledDeltaTime);
        }
    }
}
