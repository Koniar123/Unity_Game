using UnityEngine;

public class ShipInput : MonoBehaviour
{
    private SailingInputActions inputActions;

    public float SailOpeningInput => inputActions.Player.SailOpening.ReadValue<float>();

    public float SailRotationInput => inputActions.Player.SailRotation.ReadValue<float>();

    private void Awake()
    {
        inputActions = new SailingInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnDestroy()
    {
        inputActions.Dispose();
    }
}