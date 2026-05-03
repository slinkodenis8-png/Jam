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

    public AnimationCurve xSpeedCurve = AnimationCurve.EaseInOut(1, 0, 0, 1f);
    public AnimationCurve ySpeedCurve = AnimationCurve.Linear(1, 0, 1f, 1f);

    [Header("Targeting")]
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private int piercing = 2;

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
            bullet.Initialize(bulletData, direction);
        }
        else
        {
            Debug.LogError("Projectile component not found on bullet prefab!");
        }
    }
}

[System.Serializable]
public struct BulletSettings
{
    public float xSpeed;
    public float ySpeed;
    public float lifetime;
    public int damage;
    public AnimationCurve forwardSpeedCurve;
    public AnimationCurve sidewaysSpeedCurve;

    public static BulletSettings Default => new BulletSettings
    {
        xSpeed = 10f,
        ySpeed = 2f,
        lifetime = 5f,
        damage = 10,
        forwardSpeedCurve = new AnimationCurve(new Keyframe(0, 10), new Keyframe(1, 10)),
        sidewaysSpeedCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0))
    };
}
