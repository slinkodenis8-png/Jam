using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.GetComponent<IDamageable>().TakeDamage(1);
            GetComponent<IProjectile>().DestroySelf();
        }
    }
}
