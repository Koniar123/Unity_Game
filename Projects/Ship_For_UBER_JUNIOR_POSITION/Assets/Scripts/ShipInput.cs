using UnityEngine;

public class ShipInput : MonoBehaviour
{
    private SailingInputActions _inputActions;

    public float SailOpeningInput { get; private set;}
    public float SailRotationInput { get; private set;}

    private void Awake()
    {
        _inputActions = new SailingInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void Update()
    {
        SailOpeningInput = _inputActions.Player.SailOpening.ReadValue<float>();
        SailRotationInput = _inputActions.Player.SailRotation.ReadValue<float>();

        Debug.Log(
            $"Sail Opening: {SailOpeningInput}, Sail Rotation: {SailRotationInput}");
    }
}
