using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningController : MonoBehaviour
{
    private float blinkDuration = 1f;
    private Image caution;
    private Transform parentHook;
    private Camera mainCam;

    private static readonly WaitForSeconds BlinkWait = new WaitForSeconds(0.1f);

    private void Awake()
    {
        caution = GetComponent<Image>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }

    public void Initialize(Transform parent)
    {
        parentHook = parent;
    }

    private void Update()
    {
        if (parentHook != null)
        {
            if (mainCam == null) mainCam = Camera.main;
            Vector3 screenPos = mainCam.WorldToScreenPoint(parentHook.position);
            transform.position = new Vector3(screenPos.x, transform.position.y, transform.position.z);
        }
    }

    private IEnumerator Blink()
    {
        float elapsed = 0f;
        bool visible = true;

        while (elapsed < blinkDuration)
        {
            visible = !visible;
            if (caution != null)
                caution.enabled = visible;

            yield return BlinkWait;
            elapsed += 0.1f;
        }

        Destroy(gameObject);
    }
}

