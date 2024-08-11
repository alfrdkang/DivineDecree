using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnSceneLoad : MonoBehaviour
{
    private PlayableDirector playableDirector;
    private string timelinePlayedKey = "TimelinePlayed";

    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

        // Check if the timeline has already been played
        if (PlayerPrefs.GetInt(timelinePlayedKey, 0) == 0)
        {
            if (playableDirector != null)
            {
                playableDirector.Play();
                PlayerPrefs.SetInt(timelinePlayedKey, 1); // Mark timeline as played
                PlayerPrefs.Save(); // Save the PlayerPrefs changes
            }
            else
            {
                Debug.LogWarning("PlayableDirector component not found on this GameObject.");
            }
        }
    }
}