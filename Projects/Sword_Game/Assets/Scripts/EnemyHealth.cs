using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    
    private float currentHealth;
    private bool isDead;

    public bool IsDead => isDead; 

    void Start()
    {
        currentHealth = health;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log ("Enemy took " + damage + " damage. Remaining health " + currentHealth);

        if (currentHealth <= 0f && !isDead)
        {
            isDead = true; 
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
