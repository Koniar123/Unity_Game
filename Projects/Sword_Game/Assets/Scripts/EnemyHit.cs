using UnityEngine;
using System.Collections.Generic;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private bool canDealDamage = false;
    private HashSet<PlayerHealth> hitPlayers = new HashSet<PlayerHealth>();

    public void EnableHitbox()
    {
        canDealDamage = true;
        hitPlayers.Clear();
    }

    public void DisableHitbox()
    {
        canDealDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDealDamage)
            return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player == null)
            return;

        if (hitPlayers.Contains(player))
            return;

        PlayerCombat playerCombat = player.GetComponentInParent<PlayerCombat>();


        if (playerCombat != null && playerCombat.IsBlocking)
        {
            Debug.Log("Blokuje");
            hitPlayers.Add(player);
            return;
        }

        hitPlayers.Add(player);

        player.TakeDamage(damage);
    }
}