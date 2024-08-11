using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTeleporter : MonoBehaviour
{
    // The start position to teleport the player to
    public Vector3 startPosition = new Vector3(0, 0, 0);

    public GameObject player;

    private void Start()
    {
        player = ThirdPersonShooter.instance.gameObject;
        TeleportPlayerToStart();
    }

    private void TeleportPlayerToStart()
    {
        if (player != null)
        {
            player.transform.position = startPosition;
        }
        else
        {
            Debug.LogError("Player GameObject is not assigned!");
        }
    }

    // Optional: If you want to teleport the player each time a new scene is loaded
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TeleportPlayerToStart();
    }
}
