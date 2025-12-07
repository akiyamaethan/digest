using UnityEngine;

public class AutoSwim : MonoBehaviour
{
    private int direction = 0;
    private float speed = 0.5f;
    [SerializeField] private int hpValue = 1;  // How much HP this fish gives

    public void initalize(string dir, int height)
    {
        Vector3 currentPos = transform.position;
        currentPos.y = height;
        transform.position = currentPos;

        if (dir == "left")
        {
            direction = -1;
        }
        else if (dir == "right")
        {
            direction = 1;
        }
    }

    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += direction * speed * Time.fixedDeltaTime;
        transform.position = currentPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Fire the HP gain event - any subscriber will handle it
        GameEvents.OnHPGain(hpValue);
        Destroy(gameObject);
    }
}
