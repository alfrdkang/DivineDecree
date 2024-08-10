/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 29/06/2024
 * Date Modified: 29/06/2024
 * Description: Pause Menu Functions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the functionality of the pause menu, including pausing, resuming, and restarting the game.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; // Reference to the GameManager
    [SerializeField] private StarterAssetsInputs _inputs; // Reference to player input controls
    [SerializeField] private GameObject pauseMenu; // Reference to the pause menu UI
    [SerializeField] private GameObject deathMenu; // Reference to the death menu UI
    [SerializeField] private GameObject optionMenu; // Reference to the option menu UI
    [SerializeField] private GameObject HUD; // Reference to the in-game HUD UI

    public bool IsPaused = false; // Flag indicating if the game is paused

    // Update is called once per frame
    private void Update()
    {
        // Check for pause input
        if (_inputs.pause)
        {
            if (IsPaused)
            {
                Resume(); // Resume the game if already paused
            }
            else
            {
                Pause(); // Pause the game if not already paused
            }
        }
    }

    /// <summary>
    /// Resumes the game from pause state.
    /// </summary>
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        pauseMenu.SetActive(false); // Disable pause menu UI
        deathMenu.SetActive(false); // Disable death menu UI
        HUD.SetActive(true); // Enable in-game HUD UI
        Time.timeScale = 1f; // Resume normal time scale
        IsPaused = false; // Game is no longer paused
        _inputs.pause = false; // Reset pause input flag
    }

    /// <summary>
    /// Pauses the game and displays the pause menu.
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0f; // Stop time
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        pauseMenu.SetActive(true); // Enable pause menu UI
        HUD.SetActive(false); // Disable in-game HUD UI
        IsPaused = true; // Game is paused
        _inputs.pause = false; // Reset pause input flag
    }

    /// <summary>
    /// Restarts the game from the beginning.
    /// </summary>
    public void Restart()
    {
        Resume(); // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor
        Scene scene = SceneManager.GetActiveScene(); // Get current scene
        SceneManager.LoadScene(scene.name); // Reload current scene

        gameManager.playerHealth = 75; // Reset player health (example value)
    }
}
