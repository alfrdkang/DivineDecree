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
    public float playerHealth = 75f;

    /// <summary>
    /// The player's maximum health.
    /// </summary>
    private float playerMaxHealth = 75f;

    [SerializeField] private GameObject player;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private StarterAssetsInputs _inputs;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject bgBlur;

    private void FixedUpdate()
    {
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
        playerHealthBar.fillAmount = playerHealth / playerMaxHealth;
    }   
}