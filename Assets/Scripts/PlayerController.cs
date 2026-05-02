using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private float lowerSpeed;
    private Vector2 Movement;
    private Vector2 SaveMove;
    private Rigidbody2D rb;
    private bool isSlowingDown = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lowerSpeed = speed/10;
    }

    private void Update()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");

      
        Movement.Normalize(); 
    }

    private void FixedUpdate()
    {
        if (Movement == Vector2.zero)
        {
            
            if (!isSlowingDown)
            {
                isSlowingDown = true;
                SaveMove = rb.linearVelocity.normalized; 
                lowerSpeed = speed/10;
            }

            if (rb.linearVelocity.magnitude > 0.1f)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * lowerSpeed * Time.fixedDeltaTime;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                isSlowingDown = false;
            }
        if(lowerSpeed > 0)
        {

                lowerSpeed -= 10;
        }
        }
        else
        {
            
            isSlowingDown = false;
            rb.linearVelocity = rb.linearVelocity.normalized * lowerSpeed * Time.fixedDeltaTime;
            rb.AddForce(Movement.normalized * speed * Time.fixedDeltaTime);
        }
    }
}