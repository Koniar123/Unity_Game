using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private PlayerAttackDirection playerAttackDirection;

    private void Awake()
    {
        playerAttackDirection = GetComponent<PlayerAttackDirection>();
    }

    public Vector3 GetAttackDirection()
    {
        Vector2 direction = playerAttackDirection.AttackDirection;

        Vector3 attackDirection = transform.forward * direction.y + transform.right * direction.x;

        return attackDirection.normalized;
    }

    public void SetAttackDirection()
    {
        Vector3 attackDirection = GetAttackDirection();

        holdPoint.forward = attackDirection;
    }
}
