using UnityEngine;
using UnityEngine.AI;

public class TreeBoss : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    public NavMeshAgent agent;
    public Animator animator;
    public float idleRange = 20f;
    public float chaseRange = 15f;
    public float attackRange = 5f;
    public float health = 100f;
    public GameObject aoeEffect;

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
        Debug.Log("is idle");
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
        Debug.Log("is chasing");
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
        Debug.Log("is walking = false");
        animator.SetBool("isWalking", false);
        animator.SetTrigger("attack1");

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            RoarAttack();

            if (health >= 26  && health <= 75)
            {
                currentState = BossState.Attack2;
            }
        }
    }

    void Attack2State(float distanceToPlayer)
    {
        animator.SetTrigger("attack2");

        if (distanceToPlayer > attackRange)
        {
            currentState = BossState.Chase;
        }
        else
        {
            ClawAttack();

            if (health>= 0 && health <= 25)
            {
                currentState = BossState.Attack3;
            }
        }
    }

    void Attack3State(float distanceToPlayer)
    {
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
        // Hide the boss
        gameObject.SetActive(false);
    }

    void RoarAttack()
    {
        Debug.Log("Roar attack dealing 5 damage");
        // Implement roar attack logic here
        // e.g., deal damage to player and surrounding area
    }

    void ClawAttack()
    {
        Debug.Log("Claw attack dealing 15 damage");
        // Implement claw attack logic here
    }

    void AoeAttack()
    {
        Debug.Log("AOE attack dealing 10 damage");
        // Implement AOE attack logic here
        Instantiate(aoeEffect, transform.position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
