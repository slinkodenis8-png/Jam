using UnityEngine;
using System.Collections;

public class TestAI : MonoBehaviour
{
    public bool drawTrace;
    public Transform target;
    public Transform firePoint;


    public BulletSettingsSO bulletData;

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
            ProjectileSpawner.Instance.FireOnce(bulletData.bulletSettings, firePoint, target, drawTrace);
            yield return new WaitForSeconds(1f);
        }
    }
}
