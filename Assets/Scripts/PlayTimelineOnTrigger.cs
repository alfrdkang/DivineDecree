using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnTrigger : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private bool hasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector == null)
        {
            Debug.LogWarning("PlayableDirector component not found on this GameObject.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            playableDirector.Play();
            hasPlayed = true;
        }
    }
}

