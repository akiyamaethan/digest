using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AutoSwim : MonoBehaviour
{
    private int direction = 0;
    private float speed = 0.5f;
    

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

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos.x += direction * speed * Time.fixedDeltaTime;
        transform.position = currentPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
        HPManager.instance.updateHP(1);
        //GameEvents.onHPChange(1);
    }
}
