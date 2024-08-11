using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnTrigger : MonoBehaviour
{
    public BurrowBoss burrowBoss;
    public GameObject cutsceneCam; // The camera for the cutscene
    public PlayableDirector playableDirector; // Reference to the PlayableDirector
    public GameObject player;
    public double cutsceneDuration = 32; // Duration of the cutscene (in seconds)

    void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;
        if (other.CompareTag("Player")) // Optional: Trigger only if the player enters
        {
            // Disable the collider to prevent multiple triggers
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

            // Activate the cutscene camera
            cutsceneCam.SetActive(true);
            GameManager.instance.HUD.SetActive(false);
           // GameManager.instance.transform.Find("MainCamera").gameObject.SetActive(false);

            // Play the cutscene
            if (playableDirector != null)
            {
                playableDirector.Play();
                cutsceneDuration = playableDirector.duration; // Get the actual duration of the cutscene
            }

            // Start coroutine to handle the end of the cutscene
            StartCoroutine(FinishCut());
        }
    }

    IEnumerator FinishCut()
    {
        // Wait for the cutscene duration
        yield return new WaitForSeconds((float)cutsceneDuration);

        // Reactivate the MainCamera
        //  GameManager.instance.transform.Find("MainCamera").gameObject.SetActive(true);

        // Show HUD again

        GameManager.instance.HUD.SetActive(true);

        burrowBoss.enabled = true;
        player.SetActive(true);
        // Deactivate the cutscene camera
        //cutsceneCam.SetActive(false);
    }
}
