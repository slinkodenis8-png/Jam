using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float dam;
    public float speed;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = speed * transform.up;
    }
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<MeltDown>().scaleX -= dam;
            collision.gameObject.GetComponent<MeltDown>().scaleY -= dam;
        }
    }
}
