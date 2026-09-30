using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [Header("Mouse")]
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump & Gravity")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Sprint")]
    [SerializeField] private float sprintSpeed = 8f;

    [Header("Head Bob")]
    [SerializeField] private float bobSpeed = 10f;
    [SerializeField] private float bobAmount = 0.05f;

    [Header("Sprint Head Bob")]
    [SerializeField] private float sprintBobSpeed = 15f;
    [SerializeField] private float sprintBobAmount = 0.1f;


    private PlayerInputActions inputActions;
    private CharacterController characterController;
    private Animator animator;
    private PlayerCombat playerCombat;

    private float xRotation;
    private float verticalVelocity;

    private float bobTimer;
    private Vector3 cameraStartPosition;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        characterController = GetComponent<CharacterController>();

        playerCombat = GetComponent<PlayerCombat>();

        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cameraStartPosition = playerCamera.transform.localPosition;
    }

    private void Update()
    {
        Look();
        MovePlayer();
        HeadBob();
    }

    private void MovePlayer()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        Vector3 movement =
            transform.forward * input.y +
            transform.right * input.x;

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        bool isSprinting = inputActions.Player.Sprint.IsPressed() && !playerCombat.IsBlocking;

        float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

        movement *= currentSpeed;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            if (inputActions.Player.Jump.WasPressedThisFrame() && !playerCombat.IsBlocking)
            {
                verticalVelocity = Mathf.Sqrt(
                    jumpHeight * -2f * gravity
                );
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        movement.y = verticalVelocity;

        characterController.Move(movement * Time.deltaTime);
    }

    private void Look()
    {
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HeadBob()
    {
        Vector2 input = inputActions.Player.Move.ReadValue<Vector2>();

        bool isMoving = input.magnitude > 0.1f;
        bool isSprinting = inputActions.Player.Sprint.IsPressed() && !playerCombat.IsBlocking;

        if (characterController.isGrounded && isMoving)
        {
            float currentBobSpeed = isSprinting ? sprintBobSpeed : bobSpeed;
            float currentBobAmount = isSprinting ? sprintBobAmount : bobAmount;

            bobTimer += Time.deltaTime * currentBobSpeed;

            float bobY = Mathf.Sin(bobTimer) * currentBobAmount;
            float bobX = Mathf.Cos(bobTimer * 0.5f) * currentBobAmount;

            playerCamera.transform.localPosition =
            cameraStartPosition + new Vector3(bobX, bobY, 0f);
        }
        else
        {
            bobTimer = 0f;

            playerCamera.transform.localPosition = Vector3.Lerp
                (
                    playerCamera.transform.localPosition,
                    cameraStartPosition,
                    Time.deltaTime * bobSpeed
                );
        }
    }
}