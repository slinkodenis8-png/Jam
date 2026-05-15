using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMeltdown>().SetCounterValue(collision.gameObject.GetComponent<PlayerMeltdown>().currentValue-damage);
        }
    }
}
