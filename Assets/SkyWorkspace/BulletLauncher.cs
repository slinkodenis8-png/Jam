using UnityEngine;
using System.Collections;

public class BulletLauncher : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Launch Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float damage = 10f;

    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(1, 0, 0, 1f);

    [Header("Targeting")]
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private int piercing = 2;

    public Transform target;

    void Start()
    {
        if (firePoint == null)
            firePoint = transform;

        StartCoroutine(FireSpread());
    }

    private IEnumerator FireOnce()
    {
        while (true)
        {
            LaunchBulletInDirection(GetDirection(firePoint, target));
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator FireSpread()
    {
        int bulletCount = 7;
        float spreadAngle = 45f;
        float burstDelay = 1f;

        while (true)
        {
            Vector3 baseDirection = GetDirection(firePoint, target);
            float angleStep = spreadAngle / (bulletCount - 1);

            for (int i = 0; i < bulletCount; i++)
            {
                float deviationAngle = -spreadAngle / 2f + (i * angleStep);
                Vector3 spreadDirection = RotateVector(baseDirection, deviationAngle);

                LaunchBulletInDirection(spreadDirection);
            }

            yield return new WaitForSeconds(burstDelay);
        }
    }

    /// <summary>
    /// Поворачивает 2D‑вектор на заданный угол (в градусах)
    /// </summary>
    private Vector3 RotateVector(Vector3 direction, float angleDegrees)
    {
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angleRadians);
        float sin = Mathf.Sin(angleRadians);

        return new Vector3(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos,
            direction.z
        );
    }


    public Vector3 GetDirection(Transform from, Transform to)
    {
        return (to.position - from.position).normalized;
    }

    public void LaunchBulletInDirection(Vector3 direction)
    {
        if (bulletPrefab == null || target == null)
        {
            Debug.LogError("Bullet prefab or target is not assigned!");
            return;
        }

        (GameObject bulletObject, bool isNew) = Pooler.PoolSpawn(bulletPrefab, firePoint.position, Quaternion.identity);

        Projectile bullet = bulletObject.GetComponent<Projectile>();

        if (bullet != null)
        {
            bullet.direction = direction;
            bullet.speed = bulletSpeed;
            bullet.maxLifetime = maxLifetime;
            bullet.damage = damage;
            bullet.damageableTag = targetTag;
            bullet.piercing = piercing;
            bullet.speedCurve = speedCurve;
        }
        else
        {
            Debug.LogError("Projectile component not found on bullet prefab!");
        }
    }
}
