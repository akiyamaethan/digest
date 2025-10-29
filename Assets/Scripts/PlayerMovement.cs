using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 input;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        if (input.x > 0.01f) 
        {
            transform.localScale = new Vector3(5,5,5);
        }
        else if (input.x < -0.01f)
        {
            transform.localScale = new Vector3(5,-5,5);
        }
        input = input.normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
        if (input.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }
}
