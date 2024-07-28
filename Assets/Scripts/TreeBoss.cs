using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TreeBoss : MonoBehaviour
{
 {
    public Transform player;
    public float attackRange = 2f;
    public float moveSpeed = 3.5f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = moveSpeed;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            agent.SetDestination(player.position);
        }
    }

    void Attack()
    {
        animator.SetTrigger("attack");
        // Add logic for attacking the player (e.g., deal damage)
    }
}

