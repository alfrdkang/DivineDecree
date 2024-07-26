/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 24/06/2024
 * Date Modified: 25/06/2024
 * Description: GameManager Script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

/// <summary>
/// Manages the game state, player health, scene transitions, and UI updates.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    /// <summary>
    /// The player's current health.
    /// </summary>
    public int playerHealth = 75;

    /// <summary>
    /// The player's maximum health.
    /// </summary>
    private int playerMaxHealth = 75;

    /// <summary>
    /// The number of objective collectibles obtained by the player.
    /// </summary>
    public int partsObtained = 0;

    /// <summary>
    /// Indicates if a new scene has been loaded.
    /// </summary>
    public bool newScene = false;

    [SerializeField] private GameObject player;
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private StarterAssetsInputs _inputs;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject bgBlur;

    /// <summary>
    /// The starting positions for each scene by index.
    /// </summary>
    public Vector3[] sceneIndexStartPos = new Vector3[]
    {
        Vector3.zero,                                           // main menu
        new Vector3(-25.1399994f, 0, -78.4000015f),             // spaceship
        new Vector3(-23.0100002f, -0.0799999982f, -114.620003f),// world
        new Vector3(8.89999962f, 43.9000015f, -200.199997f)     // cave
    };

    private void FixedUpdate()
    {
        if (newScene == true)
        {
            UpdateHealthUI();
            player.transform.localPosition = sceneIndexStartPos[SceneManager.GetActiveScene().buildIndex];
            player.transform.rotation = Quaternion.Euler(Vector3.zero);
            newScene = false;
        }

        if (_inputs.interact)
        {
            _inputs.interact = false;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }

        UpdateHealthUI();
    }

    /// <summary>
    /// Moves the player to the next scene in the build index.
    /// </summary>
    public void MoveUpScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Moves the player to a specific scene by index.
    /// </summary>
    /// <param name="sceneIndex">The index of the scene to move to.</param>
    public void MoveToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        newScene = true;

        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Applies damage to the player.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void PlayerDamage(int damage)
    {
        playerHealth -= damage;
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Handles the player's death, triggering the death menu and pausing the game.
    /// </summary>
    private void Death()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
        HUD.SetActive(false);
        bgBlur.SetActive(true);
    }

    /// <summary>
    /// Handles the player winning, triggering the win menu and pausing the game.
    /// </summary>
    public void Win()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        winMenu.SetActive(true);
        HUD.SetActive(false);
        bgBlur.SetActive(true);
    }

    /// <summary>
    /// Heals the player by a specified value.
    /// </summary>
    /// <param name="healValue">The amount of health to restore.</param>
    public void PlayerHeal(int healValue)
    {
        playerHealth += healValue;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        UpdateHealthUI();
    }

    /// <summary>
    /// Updates the player's health UI elements.
    /// </summary>
    public void UpdateHealthUI()
    {
        healthText.SetText(playerHealth.ToString() + "/" + playerMaxHealth.ToString());
        playerHealthBar.value = playerHealth;
        playerHealthBar.maxValue = playerMaxHealth;
    }   
}