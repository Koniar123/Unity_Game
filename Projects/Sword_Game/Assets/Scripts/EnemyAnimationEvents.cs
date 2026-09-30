using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private EnemyHit enemyHit;
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyHit = GetComponent<EnemyHit>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    public void EnableHitbox()
    {
        enemyHit.EnableHitbox();
    }

    public void DisableHitbox()
    {
        enemyHit.DisableHitbox();
    }

    public void FinishAttack()
    {
        enemyAI.FinishAttack();
    }
}
