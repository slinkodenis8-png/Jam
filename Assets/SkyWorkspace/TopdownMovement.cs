using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 10f;

    [Header("Rotation Settings")]
    [SerializeField] private bool smoothRotation = true;
    [SerializeField] private float rotationSmoothTime = 0.1f;
    [SerializeField] private float maxRotationSpeed = 360f; // градусов в секунду

    [Header("Input")]
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";

    private Rigidbody2D rb;
    private Vector2 inputDirection;
    private Vector2 currentVelocity;

    [Header("References")]
    public GameObject playerVisual;

    private float currentRotationAngle = 0f;
    private float rotationVelocity = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is required for TopDownCharacterController!");
            enabled = false;
            return;
        }

        rb.gravityScale = 0f;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        ReadInput();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    void ReadInput()
    {
        float horizontal = Input.GetAxisRaw(horizontalAxis);
        float vertical = Input.GetAxisRaw(verticalAxis);

        inputDirection = new Vector2(horizontal, vertical).normalized;
    }

    void ApplyMovement()
    {
        Vector2 targetVelocity = inputDirection * maxSpeed;

        if (inputDirection.magnitude > 0.1f)
        {
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        rb.linearVelocity = currentVelocity;

        RotateTowardsMovement(playerVisual.transform);
    }

    void RotateTowardsMovement(Transform target)
    {
        if (currentVelocity.magnitude <= 0.1f)
        {
            return;
        }

        float targetAngle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;

        if (smoothRotation)
        {
            currentRotationAngle = Mathf.SmoothDampAngle(
                currentRotationAngle,
                targetAngle,
                ref rotationVelocity,
                rotationSmoothTime,
                maxRotationSpeed,
                Time.fixedDeltaTime
            );
        }
        else
        {
            currentRotationAngle = targetAngle;
        }

        target.localRotation = Quaternion.Euler(0f, 0f, currentRotationAngle);
    }

    public void SetSpeed(float newSpeed)
    {
        maxSpeed = Mathf.Max(0f, newSpeed);
    }

    public void SetAcceleration(float newAcceleration)
    {
        acceleration = Mathf.Max(0f, newAcceleration);
    }

    public void SetSmoothRotation(bool enableSmooth)
    {
        smoothRotation = enableSmooth;
    }

    public void SetRotationSmoothTime(float smoothTime)
    {
        rotationSmoothTime = Mathf.Max(0.01f, smoothTime);
    }

    public void SetMaxRotationSpeed(float speed)
    {
        maxRotationSpeed = Mathf.Max(1f, speed);
    }

    public Vector2 GetCurrentVelocity() => currentVelocity;

    public Vector2 GetInputDirection() => inputDirection;

    public float GetCurrentRotationAngle() => currentRotationAngle;
}
