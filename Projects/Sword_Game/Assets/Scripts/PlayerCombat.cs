using System.Linq.Expressions;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown = 0.5f;

    private PlayerInputActions inputActions;
    private Animator animator;

    private float lastAttackTime;
    private bool isBlocking;

    public bool IsBlocking => isBlocking;

    private void Awake()
    {
        inputActions = new PlayerInputActions();

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

        animator.SetTrigger("Attack");
    }

    private void Block()
    {
        isBlocking = inputActions.Player.Block.IsPressed();

        animator.SetBool("Block", isBlocking);
    }
}
