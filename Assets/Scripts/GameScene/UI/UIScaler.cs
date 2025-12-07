using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ensures consistent UI scaling across all display types and resolutions.
/// Attach this script to any Canvas GameObject - it will configure the CanvasScaler automatically.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class UIScaler : MonoBehaviour
{
    [Header("Reference Resolution")]
    [Tooltip("The resolution the UI was designed for")]
    [SerializeField] private Vector2 referenceResolution = new Vector2(1920, 1080);

    [Header("Scaling Settings")]
    [Tooltip("0 = match width, 1 = match height, 0.5 = balanced")]
    [Range(0f, 1f)]
    [SerializeField] private float matchWidthOrHeight = 0.5f;

    [Tooltip("Pixels per unit for sprites in the UI")]
    [SerializeField] private float referencePixelsPerUnit = 100f;

    private CanvasScaler canvasScaler;

    private void Awake()
    {
        ConfigureCanvasScaler();
    }

    private void OnValidate()
    {
        // Update in editor when values change
        ConfigureCanvasScaler();
    }

    private void ConfigureCanvasScaler()
    {
        if (canvasScaler == null)
        {
            canvasScaler = GetComponent<CanvasScaler>();
        }

        if (canvasScaler == null) return;

        // Set to Scale With Screen Size mode - this is the key setting for consistent scaling
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = referenceResolution;
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
        canvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
    }

    /// <summary>
    /// Call this if you need to reconfigure the scaler at runtime
    /// </summary>
    public void RefreshScaling()
    {
        ConfigureCanvasScaler();
    }

    /// <summary>
    /// Set reference resolution at runtime (useful for different UI layouts)
    /// </summary>
    public void SetReferenceResolution(Vector2 resolution)
    {
        referenceResolution = resolution;
        ConfigureCanvasScaler();
    }

    /// <summary>
    /// Adjust the width/height match ratio at runtime
    /// </summary>
    public void SetMatchWidthOrHeight(float value)
    {
        matchWidthOrHeight = Mathf.Clamp01(value);
        ConfigureCanvasScaler();
    }
}
