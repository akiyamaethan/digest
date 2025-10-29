using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody2D))]
public class FishFollowMouse : MonoBehaviour
{
    [SerializeField] public float speed = 4f;
    [SerializeField] public float rotationSpeed = 20f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - rb.position).normalized;


        if (direction.x > 0)
        {
            transform.localScale = new Vector3(5,5,5);
        }
        else
            transform.localScale = new Vector3(5, -5, 5);


        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.fixedDeltaTime));
        float distance = Vector2.Distance(rb.position, mousePos);
        if (distance < .9f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }
}