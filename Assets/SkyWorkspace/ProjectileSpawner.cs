using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    public static ProjectileSpawner Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void FireOnce(BulletSettings bulletData, Transform firePoint, Transform target)
    {
        LaunchBulletInDirection(bulletData, firePoint.position, GetDirection(firePoint, target));
    }

    // public void FireFixedSpread(BulletSettings bulletData, Transform firePoint, Transform target, int bulletCount = 5, float spreadAngle = 45f)
    // {
    //     Vector3 baseDirection = GetDirection(firePoint, target);
    //     float angleStep = spreadAngle / (bulletCount - 1);

    //     for (int i = 0; i < bulletCount; i++)
    //     {
    //         float deviationAngle = -spreadAngle / 2f + (i * angleStep);
    //         Vector3 spreadDirection = RotateVector(baseDirection, deviationAngle);

    //         LaunchBulletInDirection(bulletData, firePoint.position, spreadDirection);
    //     }
    // }

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

    private void LaunchBulletInDirection(BulletSettings bulletData, Vector3 from, Vector3 direction)
    {
        if (bulletData.bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab or target is not assigned!");
            return;
        }

        (GameObject bulletObject, bool isNew) = Pooler.PoolSpawn(bulletData.bulletPrefab, from, Quaternion.identity);

        IProjectile bullet = bulletObject.GetComponent<IProjectile>();

        if (bullet != null)
        {
            bullet.Initialize(bulletData, direction);
        }
        else
        {
            Debug.LogError("Projectile component not found on bullet prefab!");
        }
    }
}
