using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TreeBoss : MonoBehaviour
{
    public Transform Player;
    public NavMeshAgent agent;
    public Animator animator;
    public float idleRange = 20f;
    public float chaseRange = 15f;
    public float attackRange = 5f;
    public int health = 1200;
    public GameObject aoeEffect;
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
        Player = GameManager.instance.transform.Find("PlayerArmature");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        lastAttack1Time = -attack1Cooldown;  // Initialize so the boss can attack immediately
        lastAttack2Time = -attack2Cooldown;
        lastAttack3Time = -attack3Cooldown;
        UpdateHealthBar(); // Initialize the health bar
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(Player.position, transform.position);

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
            GameManager.instance.AddExperience(70);
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
            agent.SetDestination(Player.position);
        }
    }

    void Attack1State(float distanceToPlayer)
    {
        if (Time.time - lastAttack1Time >= attack1Cooldown)
        {
            animator.SetBool("isWalking", false);
            animator.SetTrigger("attack1");
            RoarAttack();
            lastAttack1Time = Time.time;

            if (health >= 26 && health <= 75)
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
            ClawAttack();
            lastAttack2Time = Time.time;

            if (health >= 0 && health <= 25)
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
            AoeAttack();
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

    void RoarAttack()
    {
        if (IsPlayerInAttackRange())
        {
            GameManager.instance.PlayerDamage(5);
            Debug.Log("Roar attack dealing 5 damage");
        }
    }

    void ClawAttack()
    {
        if (IsPlayerInAttackRange())
        {
            GameManager.instance.PlayerDamage(15);
            Debug.Log("Claw attack dealing 15 damage");
        }
    }

    void AoeAttack()
    {
        if (IsPlayerInAttackRange())
        {
            GameManager.instance.PlayerDamage(10);
            Instantiate(aoeEffect, transform.position, Quaternion.identity);
            Debug.Log("AOE attack dealing 10 damage");
        }
    }

    bool IsPlayerInAttackRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform == Player)
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar(); // Update the health bar whenever the boss takes damage
        Debug.Log("taken damage");
    }

    // Method to update the health bar
    public void UpdateHealthBar()
    {
        if (healthCanvas != null)
        {
            if (healthBar != null)
            {
                healthBar.fillAmount = (float)health / 1200f; // Assuming max health is 1200
            }
        }
    }
}
