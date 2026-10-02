using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerAttackDirection playerAttackDirection;
    private PlayerInputActions inputActions;
    private PlayerAttackController playerAttackController;
    private Animator animator;

    private float lastAttackTime;
    private bool isBlocking;

    public bool IsBlocking => isBlocking;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        playerAttackDirection = GetComponent<PlayerAttackDirection>();
        playerAttackController = GetComponent<PlayerAttackController>();
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
    
    void Update()
    {
        Attack();
        Block();
    }
    
    private void Attack()
    {
        if (isBlocking)
            return;
        
        if (!inputActions.Player.Attack.WasPressedThisFrame())
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        float attackAngle = playerAttackDirection.AttackAngle;

        Vector3 worldAttackDirection = playerAttackController.GetAttackDirection();

        Debug.Log("World Attack Direction: " + worldAttackDirection);

        Debug.Log("Attack Angle: " + attackAngle);

        playerAttackController.SetAttackDirection();

        animator.SetFloat("AttackAngle", attackAngle);

        animator.SetTrigger("Attack");
    }

    private void Block()
    {
        isBlocking = inputActions.Player.Block.IsPressed();

        animator.SetBool("Block", isBlocking);
    }
}
