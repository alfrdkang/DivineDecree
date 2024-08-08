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

    public int playerHealthRegenerationPerSecond = 2;

    private bool canRegen = true;

    public float LerpDuration = 0.5f;

    private Coroutine healthBarLerpCoroutine;
    private Coroutine experienceBarLerpCoroutine;

    [SerializeField] private GameObject player;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private StarterAssetsInputs _inputs;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject bgBlur;

    [SerializeField] AnimationCurve experienceCurve;

    public int currentLevel;
    public int totalExperience;
    public int previousLevelsExperience;
    public int nextLevelsExperience;

    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] Image experienceBar;

    private void Update()
    {
        //debug
        if (Input.GetMouseButtonDown(0))
        {
            AddExperience(10);
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
        UpdateLevel();

        StartCoroutine(HealthRegenerationPerSecond(playerHealthRegenerationPerSecond));
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

        canRegen = false;
        StartCoroutine(RegenTimeout());
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

    private IEnumerator RegenTimeout()
    {
        yield return new WaitForSeconds(3f);
        canRegen = true;
        StartCoroutine(HealthRegenerationPerSecond(playerHealthRegenerationPerSecond));
    }

    private IEnumerator HealthRegenerationPerSecond(int regeneration)
    {
        while (canRegen)
        {
            yield return new WaitForSeconds(1f);
            PlayerHeal(regeneration);
        }
    }

    /// <summary>
    /// Updates the player's health UI elements.
    /// </summary>
    public void UpdateHealthUI()
    {
        healthText.SetText(playerHealth.ToString() + "/" + playerMaxHealth.ToString());

        if (healthBarLerpCoroutine != null)
        {
            StopCoroutine(healthBarLerpCoroutine);
        }
        healthBarLerpCoroutine = StartCoroutine(LerpHealthBar(playerHealth / playerMaxHealth));
    }

    private IEnumerator LerpHealthBar(float targetFillAmount)
    {
        float startFillAmount = playerHealthBar.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < LerpDuration)
        {
            elapsedTime += Time.deltaTime;
            playerHealthBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / LerpDuration);
            yield return null;
        }

        playerHealthBar.fillAmount = targetFillAmount;
    }

    private IEnumerator LerpExperienceBar(float targetFillAmount)
    {
        float startFillAmount = experienceBar.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < LerpDuration)
        {
            elapsedTime += Time.deltaTime;
            experienceBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, elapsedTime / LerpDuration);
            yield return null;
        }

        experienceBar.fillAmount = targetFillAmount;
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateExperienceUI();
    }

    void CheckForLevelUp()
    {
        if (totalExperience >= nextLevelsExperience)
        {
            currentLevel++;
            UpdateLevel();

            ItemChoice.instance.itemChoiceUI.SetActive(true);
            ItemChoice.instance.DisplayItemChoices();
        }
    }

    void UpdateLevel()
    {
        previousLevelsExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelsExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateExperienceUI();
    }

    void UpdateExperienceUI()
    {
        int start = totalExperience - previousLevelsExperience;
        int end = nextLevelsExperience - previousLevelsExperience;

        experienceText.text = "Level " + currentLevel.ToString() + ": " + start + " / " + end + " XP";

        if (experienceBarLerpCoroutine != null)
        {
            StopCoroutine(experienceBarLerpCoroutine);
        }
        experienceBarLerpCoroutine = StartCoroutine(LerpExperienceBar((float)start / (float)end));
    }
}