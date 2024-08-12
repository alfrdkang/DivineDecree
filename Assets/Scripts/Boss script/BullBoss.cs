
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BullBoss : MonoBehaviour
{
    public Transform player;
    public Transform spawnPoint;
    private NavMeshAgent agent;
    private Animator animator;
    public float idleRange = 20f;
    public float chaseRange = 15f;
    public float attackRange = 5f;
    public int health = 2000;
    public Canvas healthCanvas; // Reference to the Canvas that contains the health bar
    public Image healthBar; // Reference to the Foreground in health bar

    public float attack1Cooldown = 5f;
    public float attack2Cooldown = 7f;
    public float attack3Cooldown = 10f;
    private float lastAttack1Time;
    private float lastAttack2Time;
    private float lastAttack3Time;

    public enum BossState { Idle, Chase, Attack1, Attack2, Attack3, Dead }
    public BossState currentState = BossState.Idle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastAttack1Time = -attack1Cooldown;  // Initialize so the boss can attack immediately
        lastAttack2Time = -attack2Cooldown;
        lastAttack3Time = -attack3Cooldown;
        UpdateHealthBar(); // Initialize the health bar
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
        if (Time.time - lastAttack1Time >= attack1Cooldown)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("attack1");
            Attack1();
            lastAttack1Time = Time.time;

            if (health <= 1500)
            {
                currentState = BossState.Attack2;
            }
            else if (distanceToPlayer > attackRange)
            {
                currentState = BossState.Chase;
            }
        }
    }

    void Attack2State(float distanceToPlayer)
    {
        if (Time.time - lastAttack2Time >= attack2Cooldown)
        {
            animator.SetTrigger("attack2");
            Attack2();
            lastAttack2Time = Time.time;

            if (health <= 500)
            {
                currentState = BossState.Attack3;
            }
            else if (distanceToPlayer > attackRange)
            {
                currentState = BossState.Chase;
            }
        }
    }


    void Attack3State(float distanceToPlayer)
    {
        if (Time.time - lastAttack3Time >= attack3Cooldown)
        {
            animator.SetTrigger("attack3");
            Attack3();
            lastAttack3Time = Time.time;

            if (distanceToPlayer > attackRange)
            {
                currentState = BossState.Chase;
            }
        }
    }

    void DeadState()
    {
        animator.SetBool("isDead", true);
        gameObject.SetActive(false);
    }

    void Attack1()
    {
        GameManager.instance.PlayerDamage(20); // Roar attack, dealing 20 damage
        Debug.Log("Attack1 dealing 20 damage");
    }

    void Attack2()
    {
        GameManager.instance.PlayerDamage(30); // Claw attack, dealing 30 damage
        Debug.Log("Attack2 dealing 30 damage");
    }

    void Attack3()
    {
        GameManager.instance.PlayerDamage(50); // AOE attack, dealing 50 damage and summoning minions
        Debug.Log("Attack3 dealing 50 damage and summoning minions");
        // Summon minions logic here
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar(); // Update the health bar whenever the boss takes damage
        Debug.Log("Taken damage");
    }

    public void UpdateHealthBar()
    {
        if (healthCanvas != null)
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)health / 2000f; // Assuming max health is 2000
            }
        }
    }
}