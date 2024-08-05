using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnSceneLoad : MonoBehaviour
{
    private PlayableDirector playableDirector;

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        if (playableDirector != null)
        {
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("PlayableDirector component not found on this GameObject.");
        }
    }
}

