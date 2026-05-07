using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damage = 1;
    public bool selfDestroyOnHit;
    void OnTriggerEnter2D(Collider2D coll)
    {
        IDamageable target = coll.GetComponent<IDamageable>();
        target?.TakeDamage(damage);

        if (selfDestroyOnHit)
            GetComponent<IProjectile>().DestroySelf();
    }
}
