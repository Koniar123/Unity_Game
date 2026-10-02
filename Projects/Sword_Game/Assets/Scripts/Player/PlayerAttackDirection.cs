using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackDirection : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private Vector2 attackDirection;
    private float attackAngle;

    [Header("Direction")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float maxDistance = 300f;

    private Vector2 virtualMousePosition;
    private bool wasChoosingDirection;

    public Vector2 AttackDirection => attackDirection;
    public float AttackAngle => attackAngle;

    public bool IsChoosingDirection => inputActions.Player.AttackDirection.IsPressed();

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        attackAngle = 0f;

        UpdateAttackDirection();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        bool choosingDirection = IsChoosingDirection;

        if (choosingDirection && !wasChoosingDirection)
        {
            virtualMousePosition = Vector2.zero;

            Debug.Log("Started choosing attack direction");
        }

        if (choosingDirection)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            virtualMousePosition += mouseDelta * mouseSensitivity;

            virtualMousePosition = Vector2.ClampMagnitude(virtualMousePosition, maxDistance);

            if (virtualMousePosition.sqrMagnitude > 0.01f)
            {
                UpdateAttackDirection();

                Debug.Log("Attack Angle: " + attackAngle);

                Debug.Log("Attack Direction: " + attackDirection);
            }
        }

        if (!choosingDirection && wasChoosingDirection)
        {
            Debug.Log("Attack direction selected: " + attackAngle);
        }

        wasChoosingDirection = choosingDirection;
    }

    private void UpdateAttackDirection()
    {
        attackAngle = Mathf.Atan2(virtualMousePosition.x, virtualMousePosition.y) * Mathf.Rad2Deg;

        float angleInRadians = attackAngle * Mathf.Deg2Rad;

        attackDirection = new Vector2(Mathf.Sin(angleInRadians),Mathf.Cos(angleInRadians));
    }
}
