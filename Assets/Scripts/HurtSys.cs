using UnityEngine;

public class HurtSys : MonoBehaviour
{
    public int damage;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("ouch");
           GetComponent<PlayerMeltdown>().SetCounterValue(GetComponent<PlayerMeltdown>().currentValue - damage);
           collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    
    }

