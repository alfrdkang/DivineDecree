using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class RobotBehaviour : MonoBehaviour
{
    public Transform target; // The point the robot should move to
    private NavMeshAgent agent;
    public float normalSpeed = 3.5f; // Set this to the normal speed of your NavMeshAgent

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            agent.speed = 0; // Set the speed to zero initially
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (agent != null && target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void EnableMovement()
    {
        if (agent != null)
        {
            agent.speed = normalSpeed; // Restore the normal speed
        }
    }
}


