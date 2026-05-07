using UnityEngine;

public class TopDownGun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform gunTransform; // Оружие, которое будет поворачиваться
    [SerializeField] private Transform gunPoint;     // Точка, откуда выпускается снаряд
    [SerializeField] private GameObject projectilePrefab; // Префаб снаряда
    [SerializeField] private float fireRate = 0.5f;  // Задержка между выстрелами

    [SerializeField] private BulletSettingsSO projectileSO;

    private float nextFireTime = 0f;

    private void Awake()
    {
        if (gunTransform == null)
            gunTransform = transform;
    }

    void Update()
    {
        RotateTowardsMouse();

        if (Input.GetMouseButton(0) && CanFire())
        {
            Fire();
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        Vector2 direction = mousePosition - gunTransform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        gunTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private bool CanFire()
    {
        return Time.time >= nextFireTime;
    }

    public void Fire()
    {
        if (projectilePrefab == null || gunPoint == null)
        {
            Debug.LogError("GunPoint or Projectile Prefab is not assigned!");
            return;
        }

        (GameObject bulletGO, bool isNew) = Pooler.PoolSpawn(projectilePrefab, gunPoint.position, gunPoint.rotation);

        IProjectile bullet = bulletGO.GetComponent<IProjectile>();

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector2 shootDirection = (mousePosition - gunPoint.position).normalized;

        bullet.Initialize(projectileSO.bulletSettings, shootDirection);

        
        nextFireTime = Time.time + fireRate;
    }


// #if UNITY_EDITOR
//     private void OnDrawGizmos()
//     {
//         DrawGizmos();
//     }

//     private void DrawGizmos()
//     {
//         if (gunPoint != null)
//         {
//             Gizmos.color = Color.red;
//             Gizmos.DrawSphere(gunPoint.position, 0.1f);
//             Gizmos.DrawWireSphere(gunPoint.position, 0.2f);
//         }

//         if (gunTransform != null)
//         {
//             Gizmos.color = Color.blue;
//             Gizmos.DrawLine(gunTransform.position, gunTransform.position + gunTransform.right * 1f);
//         }
//     }
// #endif
}
