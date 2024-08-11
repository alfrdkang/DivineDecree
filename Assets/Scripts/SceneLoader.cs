using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private Animator animator;

    public string sceneToLoad;

    public float loadDelay = 0f;

    private void Start()
    {
        animator = GameManager.instance.gameObject.transform.Find("HUD").Find("Crossfade").GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            animator.SetTrigger("Start");
            Invoke("LoadScene", loadDelay);
        }
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            animator.SetTrigger("End");
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Scene name is not set in the Inspector!");
        }
    }
}
