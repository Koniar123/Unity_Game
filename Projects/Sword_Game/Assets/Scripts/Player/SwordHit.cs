using UnityEngine;
using System.Collections.Generic;

public class SwordHit : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private bool canDealDamage = false;

    private HashSet<EnemyHealth> hitEnemies = new HashSet<EnemyHealth>();

    public void EnableHitbox()
    {
        canDealDamage = true;
        hitEnemies.Clear();
    }

    public void DisableHitbox()
    {
        canDealDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage)
            return;

        EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();

        if (enemy == null)
            return;

        if (hitEnemies.Contains(enemy))
            return;
        
        hitEnemies.Add(enemy);

        enemy.TakeDamage(damage);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
