using UnityEngine;
using System.Collections;

public class TestAI : MonoBehaviour
{
    public bool drawTrace;
    public GameObject tracePrefab;
    public Transform target;
    public Transform firePoint;
    public BulletSettingsSO bulletData;


    public float shotCooldown = 1f;

    void Start()
    {
        if (firePoint == null)
            firePoint = transform;

        StartCoroutine(Fire());
    }

    private IEnumerator Fire()
    {
        while (true)
        {
            if (drawTrace)
            {
                TraceManager.Instance.DrawLineOverTime(firePoint.position, target.position, tracePrefab, 0.9f, 0.3f, 0.3f);   
            }
            ProjectileSpawner.Instance.FireOnce(bulletData.bulletSettings, firePoint, target);
            yield return new WaitForSeconds(shotCooldown);
        }
    }
}
