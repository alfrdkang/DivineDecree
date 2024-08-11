using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    private bool attacked = false;
    public int damage = 2;
    public float timeBetweenAttacks = 0.75f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerDamage(damage); // Damage player

            attacked = true; // Set attacked flag
            Invoke(nameof(ResetAtk), timeBetweenAttacks); // Reset attack timer
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!attacked)
            {
                GameManager.instance.PlayerDamage(damage); // Damage player

                attacked = true; // Set attacked flag
                Invoke(nameof(ResetAtk), timeBetweenAttacks); // Reset attack timer
            }
        }
    }

    private void ResetAtk()
    {
        attacked = false; // Reset attack flag
    }
}
