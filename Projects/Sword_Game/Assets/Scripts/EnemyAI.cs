using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 2f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 1.5f;

    private Transform player;
    private PlayerHealth playerHealth;

    private float lastAttackTime;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            return;
        }

        player = playerObject.transform;

        playerHealth = playerObject.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("EnemyAI: Player nie ma PlayerHealth!");
        }

        Debug.Log("Enemy znalazł gracza.");
    }

    private void Update()
    {
        if (player == null)
            return;

        if (playerHealth == null)
            return;

        if (playerHealth.IsDead)
            return;

        Vector3 direction = player.position - transform.position;

        direction.y = 0f;

        float distance = direction.magnitude;

        if (distance > detectionRange)
        {
            return;
        }

        if (distance > attackRange)
        {
            ChasePlayer(direction);
        }
        else
        {
            AttackPlayer(direction);
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

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        Debug.Log("Enemy attacks the player!");

        playerHealth.TakeDamage(10f);
    }
}