using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BullBoss : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    private NavMeshAgent agent;
    private Animator animator;
    public float idleRange = 20f;
    public float chaseRange = 15f;
    public float attackRange = 5f;
    public float health = 100f;
    public GameObject minionPrefab;
    public Transform minionSpawnPoint;

    public enum BossState { Idle, Chase, Attack1, Attack2, Attack3, Dead }
    public BossState currentState = BossState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case BossState.Idle:
                IdleState(distanceToPlayer);
                break;
            case BossState.Chase:
                ChaseState(distanceToPlayer);
                break;
            case BossState.Attack1:
                Attack1State(distanceToPlayer);
                break;
            case BossState.Attack2:
                Attack2State(distanceToPlayer);
                break;
            case BossState.Attack3:
                Attack3State(distanceToPlayer);
                break;
            case BossState.Dead:
                DeadState();
                break;
        }

        if (health <= 0 && currentState != BossState.Dead)
        {
            currentState = BossState.Dead;
        }
    }

    void IdleState(float distanceToPlayer)
    {
        Debug.Log("BullBoss is idle");
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalking", false);

        if (distanceToPlayer <= chaseRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            agent.SetDestination(spawnPoint.position);
        }
    }

    void ChaseState(float distanceToPlayer)
    {
        Debug.Log("BullBoss is chasing");
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", true);

        if (distanceToPlayer > idleRange)
        {
            currentState = BossState.Idle;
        }
        else if (distanceToPlayer <= attackRange)
        {
            currentState = BossState.Attack1;
        }
        else
        {
            agent.SetDestination(player.position);
        }
    }

    void Attack1State(float distanceToPlayer)
    {
        Debug.Log("BullBoss performing attack1");
        animator.SetBool("isWalking", false);
        animator.SetTrigger("attack1");

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            RoarAttack();

            if (health <= 75f)
            {
                currentState = BossState.Attack2;
            }
        }
    }

    void Attack2State(float distanceToPlayer)
    {
        Debug.Log("BullBoss performing attack2");
        animator.SetTrigger("attack2");

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            ClawAttack();

            if (health <= 25f)
            {
                currentState = BossState.Attack3;
            }
        }
    }

    void Attack3State(float distanceToPlayer)
    {
        Debug.Log("BullBoss performing attack3");
        animator.SetTrigger("attack3");

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            AoeAttack();
        }
    }

    void DeadState()
    {
        animator.SetBool("isDead", true);
        gameObject.SetActive(false);
    }

    void RoarAttack()
    {
        Debug.Log("Roar attack dealing 5 damage");
        DealDamage(5, 10f); // Roar damage radius
    }

    void ClawAttack()
    {
        Debug.Log("Claw attack dealing 15 damage");
        DealDamage(15, attackRange);
    }

    void AoeAttack()
    {
        Debug.Log("AOE attack dealing 10 damage and summoning minions");
        DealDamage(10, attackRange);
        SpawnMinions();
    }

    void DealDamage(float damage, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Reduce player's health here
                Debug.Log("Player hit for " + damage + " damage");
            }
        }
    }

    void SpawnMinions()
    {
        Instantiate(minionPrefab, minionSpawnPoint.position, minionSpawnPoint.rotation);
        // Additional logic to handle minions
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            currentState = BossState.Dead;
        }
    }
}
