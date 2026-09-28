using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool IsDead => isDead;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0f);

        Debug.Log("Player took " + damage + " damage. Remaining health: " + currentHealth);

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
