using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private TreeBoss treeBoss;

    void Start()
    {
        // Find the TreeBoss in the scene
        treeBoss = FindObjectOfType<TreeBoss>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && treeBoss != null)
        {
            treeBoss.currentState = TreeBoss.BossState.Chase;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && treeBoss != null)
        {
            treeBoss.currentState = TreeBoss.BossState.Idle;
        }
    }
}
