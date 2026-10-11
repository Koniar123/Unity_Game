using UnityEngine;

[DefaultExecutionOrder(10)]
[RequireComponent(typeof(ShipController))]
public class ShipMovement : MonoBehaviour
{
    private const float MinApparentWindSpeed = 0.1f;
    private const float MinTurnSpeedSqr = 0.01f;

    [Header("References")]
    [SerializeField] private WindController wind;

    [Header("Ship")]
    [SerializeField] private float shipMass = 500f;
    [SerializeField] private float forwardDrag = 15f;
    [SerializeField] private float lateralDrag = 80f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float turnSpeed = 30f;

    [Header("Sail")]
    [SerializeField] private float sailArea = 10f;
    [SerializeField] private float airDensity = 1.225f;
    [SerializeField] private float sailForceCoefficient = 1f;

    private ShipController shipController;
    private Vector3 velocity;
    private float heading;

    public Vector3 Velocity => velocity;

    public float Speed => velocity.magnitude;

    public float SignedWindAcrossSail { get; private set; }

    public float ApparentWindSpeed { get; private set; }

    private void Awake()
    {
        shipController = GetComponent<ShipController>();
        heading = transform.eulerAngles.y;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 force = CalculateSailForce() + CalculateWaterDrag();

        velocity += force / Mathf.Max(shipMass, 0.01f) * deltaTime;
        velocity.y = 0f;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += velocity * deltaTime;
        TurnTowardsVelocity(deltaTime);
    }

    private Vector3 CalculateSailForce()
    {
        SignedWindAcrossSail = 0f;
        ApparentWindSpeed = 0f;

        if (wind == null)
        {
            return Vector3.zero;
        }

        // Apparent wind = true wind minus ship velocity.
        Vector3 apparentWind = wind.WindVelocity - velocity;
        apparentWind.y = 0f;

        float apparentSpeed = apparentWind.magnitude;
        ApparentWindSpeed = apparentSpeed;

        if (apparentSpeed < MinApparentWindSpeed)
        {
            return Vector3.zero;
        }

        Vector3 windDirection = apparentWind / apparentSpeed;
        Vector3 sailNormal = Vector3.Cross(Vector3.up, shipController.SailForward);

        // Signed sine of the angle of attack: positive when the wind pushes along +normal.
        float windAcrossSail = Vector3.Dot(windDirection, sailNormal);
        SignedWindAcrossSail = windAcrossSail;

        float opening = shipController.SailOpeningNormalized;

        if (opening <= 0f)
        {
            return Vector3.zero;
        }

        float dynamicPressure = 0.5f * airDensity * apparentSpeed * apparentSpeed;
        float magnitude = dynamicPressure * sailArea * sailForceCoefficient * opening * windAcrossSail;

        return sailNormal * magnitude;
    }

    private Vector3 CalculateWaterDrag()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float forwardSpeed = Vector3.Dot(velocity, forward);
        float lateralSpeed = Vector3.Dot(velocity, right);

        return -(forward * (forwardSpeed * Mathf.Abs(forwardSpeed) * forwardDrag)
               + right * (lateralSpeed * Mathf.Abs(lateralSpeed) * lateralDrag));
    }

    private void TurnTowardsVelocity(float deltaTime)
    {
        if (velocity.sqrMagnitude >= MinTurnSpeedSqr)
        {
            float targetHeading = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
            heading = Mathf.MoveTowardsAngle(heading, targetHeading, turnSpeed * deltaTime);
        }

        transform.rotation = Quaternion.Euler(0f, heading, 0f);
    }
}