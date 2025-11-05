using UnityEngine;
using UnityEngine.UI;

public class WarningController : MonoBehaviour
{
    private float blinkDuration = 1f;
    private Image caution;
    private Transform parentHook;
    void Start()
    {
        caution = GetComponent<Image>();
        StartCoroutine(blink());
    }

    public void initialize(Transform parent)
    {
        parentHook = parent;
    }

    private void Update()
    {
        if (parentHook != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(parentHook.position);
            transform.position = new Vector3(screenPos.x, transform.position.y, transform.position.z);
        }
    }

    private System.Collections.IEnumerator blink()
    {
        float elapsed = 0f;
        bool visible = true;


        while (elapsed < blinkDuration)
        {
            visible = !visible;
            if (caution != null)
                caution.enabled = visible;

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        Destroy(gameObject);
    }

}
