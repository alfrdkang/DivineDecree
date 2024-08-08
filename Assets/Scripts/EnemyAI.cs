/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 22/06/2024
 * Date Modified: 25/06/2024
 * Description: Script Attached to Enemies for NavMesh and Health
 */

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// Controls the behavior and interactions of an enemy character using NavMesh for navigation and health management.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent; // Reference to the NavMeshAgent component for navigation

    private Transform player; // Reference to the player's transform

    public LayerMask IsGround, IsPlayer; // Layers used for ground detection and player detection

    public float maxHealth; // Maximum health of the enemy
    private float health; // Current health of the enemy
    [SerializeField] private int damage; // Amount of damage dealt to the player
    [SerializeField] private Image healthbar; // UI element displaying health bar
    [SerializeField] private GameObject healthBarCanvas; // Canvas containing health bar UI
    [SerializeField] private GameObject damageText; // Text displaying damage enemy took
    private Camera cam; // Main camera reference
    private Animator animator; // Animator component reference
    private GameManager gameManager; // Reference to the game manager

    // Patrol
    private Vector3 walkPoint; // Random point for patrolling
    bool walkPointSet; // Flag indicating if a walk point is set
    public float walkPointRange; // Range within which to set walk points

    // Attack
    public float timeBetweenAttacks; // Time between consecutive attacks
    bool attacked; // Flag indicating if an attack has been performed

    // States
    public float sightRange, attackRange; // Range for sight and attack detection
    public bool playerInSight, playerInAttack; // Flags indicating if player is in sight or attack range

    private void Awake()
    {
        if (TryGetComponent(out Flying flying))
        {
            agent.baseOffset = 1.2f; // Adjust base offset for flying enemies
        }

        cam = Camera.main; // Get reference to the main camera
        animator = GetComponent<Animator>(); // Get reference to the Animator component
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find and assign player's transform
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); // Find and assign player's transform
        agent = GetComponent<NavMeshAgent>(); // Get reference to the NavMeshAgent component
        health = maxHealth; // Set initial health to maximum

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); // Find and assign GameManager component
    }

    private void Update()
    {
        if (health >= 0)
        {
            // Check if Player is in sight/attack range
            playerInSight = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
            playerInAttack = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

            // Change Enemy State
            if (!playerInSight && !playerInAttack) Patrol();
            if (playerInSight && !playerInAttack) Chase();
            if (playerInSight && playerInAttack) Attack();
        }

        // Rotate Healthbar to face the camera
        healthBarCanvas.transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }

    /// <summary>
    /// Updates the health bar UI to reflect current health status.
    /// </summary>
    /// <param name="health">Current health value</param>
    /// <param name="maxHealth">Maximum health value</param>
    public void UpdateHealthBar(float health, float maxHealth)
    {
        healthbar.fillAmount = health / maxHealth; // Update health bar fill amount
    }

    private void Patrol()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint(); // Find a new walk point if none is set
        }
        else
        {
            agent.SetDestination(walkPoint); // Set destination to the walk point
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false; // Reset walk point if reached
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check that random point is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
        {
            walkPointSet = true; // Set walk point if valid
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position); // Set destination to player's position
        transform.LookAt(player); // Rotate to face player
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player); // Rotate to face player

        //atk 
    }

    private void ResetAtk()
    {
        attacked = false; // Reset attack flag
    }

    /// <summary>
    /// Inflicts damage on the enemy and updates health bar UI.
    /// </summary>
    /// <param name="damage">Amount of damage to inflict</param>
    public void TakeDamage(int damage)
    {
        animator.Play("GetHit"); // Play hit animation
        health -= damage; // Reduce health
        UpdateHealthBar(health, maxHealth); // Update health bar UI

        DamageHitmarker indicator = Instantiate(damageText, healthBarCanvas.transform.position, Quaternion.identity).GetComponent<DamageHitmarker>();
        indicator.SetDamageText(damage);

        if (health <= 0)
        {
            agent.SetDestination(transform.position); // Stop moving
            Destroy(GetComponent<CapsuleCollider>()); // Disable collider
            animator.Play("Die"); // Play death animation
            Invoke(nameof(DestroyEnemy), 1f); // Destroy enemy after delay
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!attacked) 
            {
                animator.Play("Attack"); // Play attack animation
                gameManager.PlayerDamage(damage); // Damage player

                attacked = true; // Set attacked flag
                Invoke(nameof(ResetAtk), timeBetweenAttacks); // Reset attack timer
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!attacked)
            {
                animator.Play("Attack"); // Play attack animation
                gameManager.PlayerDamage(damage); // Damage player

                attacked = true; // Set attacked flag
                Invoke(nameof(ResetAtk), timeBetweenAttacks); // Reset attack timer
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject); // Destroy the enemy GameObject
    }
}
