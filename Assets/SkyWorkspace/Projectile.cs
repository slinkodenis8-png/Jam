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

    // References
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Collider2D coll;
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private ParticleSystem particles;

    [Header("Speed Curves")]
    public AnimationCurve forwardSpeedCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
    public AnimationCurve sidewaysSpeedCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 0));

    private bool isActive;


    public GameObject tracePrefab;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (!isActive) return;
        currentLifetime += Time.fixedDeltaTime;

        float normalizedTime = Mathf.Clamp01(currentLifetime / maxLifetime);

        float forwardMultiplier = forwardSpeedCurve.Evaluate(currentLifetime);
        float sidewaysMultiplier = sidewaysSpeedCurve.Evaluate(currentLifetime);

        Vector2 forwardMovement = transform.up * speed * forwardMultiplier;

        Vector2 sidewaysMovement = transform.right * speed * sidewaysMultiplier;

        Vector2 totalMovement = forwardMovement + sidewaysMovement;

        rb.MovePosition(rb.position + totalMovement * Time.fixedDeltaTime);
    }

    void OnEnable()
    {
        coll.enabled = true;
        spriteRenderer.enabled = true;
        trailRenderer.Clear();
        isActive = true;
        
    }

    void OnDisable()
    {
        StopAllCoroutines();

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


    public void Initialize(BulletSettings bulletSettings, Vector3 newDirection)
    {
        direction = newDirection;
        transform.up = direction;

        speed = bulletSettings.xSpeed;
        maxLifetime = bulletSettings.lifetime;
        damage = bulletSettings.damage;
        forwardSpeedCurve = bulletSettings.forwardSpeedCurve;
        sidewaysSpeedCurve = bulletSettings.sidewaysSpeedCurve;

        StartCoroutine(DestroyAfterLifetime());

        TraceManager.Instance.DrawLineOverTime(transform.position, transform.position + (direction * 12), tracePrefab, 0.9f, 0.3f, 0.3f);
    }


    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
