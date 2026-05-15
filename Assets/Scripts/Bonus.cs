using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMeltdown>().SetCounterValue(collision.gameObject.GetComponent<PlayerMeltdown>().currentValue + value);
            Destroy(gameObject);
        }
    }
}
