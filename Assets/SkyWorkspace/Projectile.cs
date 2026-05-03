using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SyncVar]
    public Vector3 direction;
    //[SyncVar]
    public float speed = 10f;
    //[SyncVar]
    public float maxLifetime;
    private float currentLifetime = 0;
    //[SyncVar]
    public float damage;
    //[SyncVar]
    public string damageableTag = "Enemy";

    // References
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D coll;
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private ParticleSystem particles;

    public int piercing = 2;

    public bool isActive;

    [HideInInspector]
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(1, 0, 0, 1f);

    void Start()
    {
        //StartCoroutine(DestroyAfterLifetime());
    }

    void FixedUpdate()
    {
        if (!isActive) return;
        currentLifetime += Time.fixedDeltaTime;

        float normalizedTime = Mathf.Clamp01(currentLifetime / maxLifetime);
        float curveValue = speedCurve.Evaluate(normalizedTime);

        transform.position += direction * speed * curveValue * Time.fixedDeltaTime;
    }

    void OnEnable()
    {
        currentLifetime = 0;
        coll.enabled = true;
        spriteRenderer.enabled = true;
        trailRenderer.Clear();
        isActive = true;

        StartCoroutine(DestroyAfterLifetime());

    }

    private void UpdateSpeed()
    {

    }

    IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSecondsRealtime(maxLifetime);
        DestroySelf();
    }

    void DestroySelf()
    {
        Pooler.PoolDespawn(gameObject);
    }
}