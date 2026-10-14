using UnityEngine;

[DefaultExecutionOrder(10)]
[RequireComponent(typeof(ShipController))]
public class ShipMovement : MonoBehaviour
{
    private const float MinApparentWindSpeed = 0.1f;
    private const float MinTurnSpeedSqr = 0.01f;

    [Header("References")]
    [SerializeField] private WindController _wind;

    [Header("Ship")]
    [SerializeField] private float _shipMass = 500f;
    [SerializeField] private float _forwardDrag = 15f;
    [SerializeField] private float _lateralDrag = 80f;
    [SerializeField] private float _maxSpeed = 10f;
    [Tooltip("Hull turn rate towards the direction of travel (degrees per second).")]
    [SerializeField] private float _turnSpeed = 30f;

    [Header("Sail")]
    [SerializeField] private float _sailArea = 10f;
    [SerializeField] private float _airDensity = 1.225f;
    [SerializeField] private float _sailForceCoefficient = 1f;

    private ShipController _shipController;
    private Vector3 _velocity;
    private float _heading;

    public Vector3 Velocity => _velocity;

    public float Speed => _velocity.magnitude;

    public float SignedWindAcrossSail { get; private set; }

    public float ApparentWindSpeed { get; private set; }

    public float WaterDragMagnitude { get; private set; }

    private void Awake()
    {
        _shipController = GetComponent<ShipController>();
        _heading = transform.eulerAngles.y;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        Vector3 waterDrag = CalculateWaterDrag();
        WaterDragMagnitude = waterDrag.magnitude;

        Vector3 force = CalculateSailForce() + waterDrag;

        _velocity += force / Mathf.Max(_shipMass, 0.01f) * deltaTime;
        _velocity.y = 0f;
        _velocity = Vector3.ClampMagnitude(_velocity, _maxSpeed);

        transform.position += _velocity * deltaTime;
        TurnTowardsVelocity(deltaTime);
    }

    private Vector3 CalculateSailForce()
    {
        SignedWindAcrossSail = 0f;
        ApparentWindSpeed = 0f;

        if (_wind == null)
        {
            return Vector3.zero;
        }

        Vector3 apparentWind = _wind.WindVelocity - _velocity;
        apparentWind.y = 0f;

        float apparentSpeed = apparentWind.magnitude;
        ApparentWindSpeed = apparentSpeed;

        if (apparentSpeed < MinApparentWindSpeed)
        {
            return Vector3.zero;
        }

        Vector3 windDirection = apparentWind / apparentSpeed;
        Vector3 sailNormal = Vector3.Cross(Vector3.up, _shipController.SailForward);

        float windAcrossSail = Vector3.Dot(windDirection, sailNormal);
        SignedWindAcrossSail = windAcrossSail;

        float opening = _shipController.SailOpeningNormalized;

        if (opening <= 0f)
        {
            return Vector3.zero;
        }

        float dynamicPressure = 0.5f * _airDensity * apparentSpeed * apparentSpeed;
        float magnitude = dynamicPressure * _sailArea * _sailForceCoefficient * opening * windAcrossSail;

        return sailNormal * magnitude;
    }

    private Vector3 CalculateWaterDrag()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float forwardSpeed = Vector3.Dot(_velocity, forward);
        float lateralSpeed = Vector3.Dot(_velocity, right);

        return -(forward * (forwardSpeed * Mathf.Abs(forwardSpeed) * _forwardDrag)
               + right * (lateralSpeed * Mathf.Abs(lateralSpeed) * _lateralDrag));
    }

    private void TurnTowardsVelocity(float deltaTime)
    {
        if (_velocity.sqrMagnitude >= MinTurnSpeedSqr)
        {
            float targetHeading = Mathf.Atan2(_velocity.x, _velocity.z) * Mathf.Rad2Deg;
            _heading = Mathf.MoveTowardsAngle(_heading, targetHeading, _turnSpeed * deltaTime);
        }

        transform.rotation = Quaternion.Euler(0f, _heading, 0f);
    }
}