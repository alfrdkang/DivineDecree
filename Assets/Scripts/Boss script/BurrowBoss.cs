using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BurrowBoss : MonoBehaviour
{
    public float maxHealth = 200f;
    public float currentHealth;
    public float damage = 10f;
    public float enhancedDamage = 20f;
    public float stunDuration = 5f;
    public float chaseRange = 15f;
    private Transform player;
    public Canvas healthCanvas;
    public Image healthBar;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isStunned = false;

    void Start()
    {
        player = GameManager.instance.transform.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();  // Initialize the health bar
    }

    void Update()
    {
        if (!isStunned && Vector3.Distance(transform.position, player.position) <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            animator.SetBool("Walking", false); // Ensure walking animation stops when not chasing
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("Walking", true);

        // Check if the boss is close enough to the player to stop and attack
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            StartCoroutine(IdleAndDamagePlayer());
        }
    }

    IEnumerator IdleAndDamagePlayer()
    {
        // Stop moving and transition to idle
        agent.isStopped = true;
        animator.SetBool("Walking", false);

        // Damage the player
        float appliedDamage = currentHealth <= maxHealth / 2 ? enhancedDamage : damage;
        GameManager.instance.PlayerDamage((int)appliedDamage);

        // Stun the boss for a duration
        isStunned = true;

        yield return new WaitForSeconds(stunDuration);

        // Resume movement after the stun
        isStunned = false;
        agent.isStopped = false;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damage;
        UpdateHealthBar();  // Update the health bar when the boss takes damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("die");
        agent.isStopped = true;
        // Add any additional logic for when the boss dies
    }

    public void UpdateHealthBar()
    {
        Debug.Log("take damage");
        if (healthCanvas != null && healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth; // Update health bar fill amount based on current health
        }
    }
}
