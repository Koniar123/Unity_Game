using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDelay = 0.4f;
    [SerializeField] private float attackDamage = 10f;

    private Transform player;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private EnemyState currentState;

    private float lastAttackTime;
    private bool isAttacking;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            return;
        }

        player = playerObject.transform;

        playerHealth = playerObject.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("EnemyAI: Player nie ma PlayerHealth!");
        }

        if (enemyHealth == null)
        {
            Debug.LogError("EnemyAI: Enemy nie ma Health!");
        }

        Debug.Log("Enemy znalazł gracza.");

        currentState = EnemyState.Idle;
    }

    private void Update()
    {
        if (player == null)
            return;

        if (playerHealth == null)
            return;

        if (enemyHealth == null)
            return;

        if (playerHealth.IsDead)
            return;

        if (enemyHealth.IsDead)
        {
            currentState = EnemyState.Dead;
            return;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Attack:
                HandleAttack();
                break;

            case EnemyState.Dead:
                return;
        }
    }

    private void ChasePlayer(Vector3 direction)
    {
        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        transform.position +=
            direction.normalized * moveSpeed * Time.deltaTime;
    }

    private void AttackPlayer(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);
        }

        lastAttackTime = Time.time;

        Debug.Log("Enemy attacks the player!");

        StartCoroutine(AttackRoutine());

        isAttacking = true;
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        Debug.Log("Enemy starts attack!");

        yield return new WaitForSeconds(attackDelay);

        if (playerHealth == null || playerHealth.IsDead)
        {
            isAttacking = false;
            yield break;
        }

        if (enemyHealth == null || enemyHealth.IsDead)
        {
            isAttacking = false;
            yield break;
        }

        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > attackRange)
        {
            Debug.Log("Player escaped the attack range.");

            isAttacking = false;
            currentState = EnemyState.Chase;
            yield break;
        }

        DealDamage();

        isAttacking = false;
        currentState = EnemyState.Chase;
    }

    private void DealDamage()
    {
        playerHealth.TakeDamage(attackDamage);

        Debug.Log(
            "Enemy dealt " +
            attackDamage +
            " damage to the player."
        );
    }

    private void HandleIdle()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance <= detectionRange)
        {
            currentState = EnemyState.Chase;
        }
    }

    private void HandleChase()
    {
        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > detectionRange)
        {
            currentState = EnemyState.Idle;
            return;
        }

        if (distance <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                currentState = EnemyState.Attack;
                return;
            }

            return;
        }

        ChasePlayer(direction);
    }

    private void HandleAttack()
    {
        if (isAttacking)
            return;

        AttackPlayer(player.position - transform.position);
    }
}