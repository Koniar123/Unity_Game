using UnityEngine;

[RequireComponent(typeof(ShipInput))]
public class ShipController : MonoBehaviour
{
    private const float MaxSailOpening = 100f;
    private const float MaxSailRotation = 45f;

    [Header("References")]
    [SerializeField] private Transform sail;
    [SerializeField] private Transform referenceFrame;

    [Header("Sail")]
    [SerializeField] private float sailOpeningSpeed = 50f;
    [SerializeField] private float sailRotationSpeed = 45f;

    private ShipInput shipInput;
    private float sailOpening;
    private float sailRotation;

    public float SailOpening => sailOpening;

    public float SailOpeningNormalized => sailOpening / MaxSailOpening;

    public float SailRotation => sailRotation;

    public Vector3 SailForward
    {
        get
        {
            Vector3 forward = sail.forward;
            forward.y = 0f;

            return forward.sqrMagnitude > 0.0001f
                ? forward.normalized
                : transform.forward;
        }
    }

    private void Awake()
    {
        shipInput = GetComponent<ShipInput>();

        if (sail == null)
        {
            Debug.LogError("ShipController: the Sail field is empty. Assign the SailPivot object.", this);
        }
        else if (sail.GetComponent<SailCloth>() == null)
        {
            Debug.LogWarning(
                "ShipController: the assigned sail has no SailCloth component. " +
                "Assign the object that has SailCloth.",
                this);
        }
    }

    private void Update()
    {
        UpdateSailOpening();
        UpdateSailRotation();
        ApplySailTransform();
    }

    private void UpdateSailOpening()
    {
        sailOpening += shipInput.SailOpeningInput * sailOpeningSpeed * Time.deltaTime;
        sailOpening = Mathf.Clamp(sailOpening, 0f, MaxSailOpening);
    }

    private void UpdateSailRotation()
    {
        sailRotation += shipInput.SailRotationInput * sailRotationSpeed * Time.deltaTime;
        sailRotation = Mathf.Clamp(sailRotation, -MaxSailRotation, MaxSailRotation);
    }

    private void ApplySailTransform()
    {
        Quaternion yaw = Quaternion.Euler(0f, sailRotation - 90f, 0f);

        if (referenceFrame != null)
        {
            // World rotation = frame rotation * yaw around the mast.
            // Ignores any odd rotation of the parents.
            sail.rotation = referenceFrame.rotation * yaw;
        }
        else
        {
            sail.localRotation = yaw;
        }
    }
}