using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TreeBoss : MonoBehaviour
{
    public Transform Player;
    public float attackRange = 10.0f; // Set your desired attack range
    private NavMeshAgent agent;
    private Animator anim;
    private Vector3 originalPosition;
    private bool isReturningToOriginalPosition = false;

    int hAngry;
    int hAttack;
    int hGrabs;
    int hThumbsUp;
    int hIdles;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        hAngry = Animator.StringToHash("Angry");
        hAttack = Animator.StringToHash("Attack");
        hGrabs = Animator.StringToHash("Grabs");
        hThumbsUp = Animator.StringToHash("ThumbsUp");
        hIdles = Animator.StringToHash("Idles");

        // Store the original position of the monster
        originalPosition = transform.position;

        StartCoroutine(AnimatePattern());
    }

    void Update()
    {
        // Check if the player is within attack range and the boss is not returning to the original position
        if (!isReturningToOriginalPosition && Vector3.Distance(transform.position, Player.position) <= attackRange)
        {
            // Start attack animation
            anim.SetTrigger(hAttack);
            agent.isStopped = true; // Stop the agent to perform the attack animation
        }
        else if (isReturningToOriginalPosition)
        {
            // Ensure the NavMeshAgent is moving to the original position
            agent.SetDestination(originalPosition);
            if (Vector3.Distance(transform.position, originalPosition) <= 0.1f)
            {
                // Stop the agent and set returning flag to false once the destination is reached
                agent.isStopped = true;
                isReturningToOriginalPosition = false;
            }
        }
        else
        {
            // Update destination to player position if not returning to original position
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idles"))
            {
                agent.isStopped = false;
                agent.SetDestination(Player.position);
            }
        }
    }

    IEnumerator AnimatePattern()
    {
        while (true)
        {
            // Angry animation
            anim.SetTrigger(hAngry);
            yield return new WaitForSeconds(5);

            // Move to original position and play idle animation
            anim.SetTrigger(hIdles);
            isReturningToOriginalPosition = true;
            agent.isStopped = false; // Ensure the agent is not stopped

            // Wait until the monster reaches the original position
            while (isReturningToOriginalPosition)
            {
                yield return null; // Wait until the monster reaches the original position
            }

            // Grabs animation
            anim.SetTrigger(hGrabs);
            yield return new WaitForSeconds(5);

            // Thumbs Up animation
            anim.SetTrigger(hThumbsUp);
            yield return new WaitForSeconds(5);
        }
    }
}
