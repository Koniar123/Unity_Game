using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private Transform sail;
    [SerializeField] private float maxSailOpening = 100f;
    [SerializeField] private float maxSailRotation = 45f;

    private ShipInput _shipInput;

    private float _sailOpening;
    private float _sailRotation;

    public float SailOpening => _sailOpening;
    public float SailRotation => _sailRotation;

    private void Awake()
    {
        _shipInput = GetComponent<ShipInput>();
    }

    private void Update()
    {
        UpdateSailOpening();
        UpdateSailRotation();
        UpdateSailTransform();
    }

    private void UpdateSailOpening()
    {
        float input = _shipInput.SailOpeningInput;

        _sailOpening += input * 50f * Time.deltaTime; // Adjust the multiplier as needed for sensitivity
        _sailOpening = Mathf.Clamp(_sailOpening, 0f, maxSailOpening); // Clamp the sail opening between 0 and maxSailOpening
    }

    private void UpdateSailRotation()
    {
        float input = _shipInput.SailRotationInput;

        _sailRotation += input * 45f * Time.deltaTime;
        _sailRotation = Mathf.Clamp(_sailRotation, -maxSailRotation, maxSailRotation);
    }

    private void UpdateSailTransform()
    {
        sail.localRotation = Quaternion.Euler(0f, _sailRotation, -90f);
    }
}
