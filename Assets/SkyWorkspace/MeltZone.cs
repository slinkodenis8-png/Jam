using UnityEngine;

public class MeltZone : MonoBehaviour
{
    public int meltPower = 1;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerMeltdown player =  coll.GetComponent<PlayerMeltdown>();
        player.meltSpeed += meltPower;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        PlayerMeltdown player =  coll.GetComponent<PlayerMeltdown>();
        player.meltSpeed -= meltPower;
    }
}
