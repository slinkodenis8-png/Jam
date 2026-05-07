using UnityEngine;
using System.Collections;

public class BulletLauncher : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Launch Settings")]
    public float bulletSpeed = 10f;
    public float maxLifetime = 5f;
    public float damage = 10f;

    public AnimationCurve xSpeedCurve = AnimationCurve.EaseInOut(1, 0, 0, 1f);
    public AnimationCurve ySpeedCurve = AnimationCurve.Linear(1, 0, 1f, 1f);

    public bool drawTrace;
    public GameObject tracePrefab;

    public Transform target;


    public BulletSettings bulletData;

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

                LaunchBulletInDirection(spreadDirection, drawTrace);
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

    public void LaunchBulletInDirection(Vector3 direction, bool drawTrace = false)
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
            bullet.Initialize(bulletData, direction);
            if (drawTrace)
            {
                TraceManager.Instance.DrawLineOverTime(transform.position, transform.position + (direction * 12), tracePrefab, 0.9f, 0.3f, 0.3f);
            }
        }
        else
        {
            Debug.LogError("Projectile component not found on bullet prefab!");
        }
    }
}
