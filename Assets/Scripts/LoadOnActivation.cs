using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnActivation : MonoBehaviour
{
    public string loadScene;

    void OnEnable()
    {
        SceneManager.LoadScene(loadScene);
    }
}