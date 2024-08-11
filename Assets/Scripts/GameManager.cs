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
    public float playerMaxHealth = 75f;

    public int extraLives = 0;

    public int playerHealthRegenerationPerSecond = 2;

    public float playerBaseDamage = 11;

    public int playerSkillDamageMultiplier = 3;

    public int regenTimeout = 0;

    public float LerpDuration = 0.5f;

    public bool bubbleShieldActive = false;
    private int bubbleShieldCount = 0;

    private Coroutine healthBarLerpCoroutine;
    private Coroutine experienceBarLerpCoroutine;

    [SerializeField] private GameObject player;
    [SerializeField] private Image playerHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private StarterAssetsInputs _inputs;
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject winMenu;
    public GameObject HUD;
    [SerializeField] private GameObject bubbleShield;

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

    public void BubbleShield()
    {
        if (!bubbleShieldActive)
        {
            Instantiate(bubbleShield, player.transform.position, player.transform.rotation, player.transform);
        }
        bubbleShieldActive = true;
        bubbleShieldCount += 1;
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
        if (bubbleShieldCount > 0)
        {
            damage -= (damage * bubbleShieldCount / 10);
        }
        playerHealth -= damage;
        UpdateHealthUI();

        if (playerHealth <= 0)
        {
            if (extraLives > 0)
            {
                extraLives -= 1;
                playerHealth = playerMaxHealth;
            } else
            {
                Death();
            }
        }

        regenTimeout = 3; // change this for regen timeout timer in seconds
    }

    /// <summary>
    /// Handles the player's death, triggering the death menu and pausing the game.
    /// </summary>
    private void Death()
    {
        StarterAssetsInputs.instance.inputs = false;
        StartCoroutine(DeathSlowMo());
    }

    private IEnumerator DeathSlowMo()
    {
        HUD.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        deathMenu.SetActive(true);
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

    private IEnumerator HealthRegenerationPerSecond(int regeneration)
    {
        while (true)
        {
            if (regenTimeout == 0)
            {
                yield return new WaitForSeconds(1f);
                PlayerHeal(regeneration);
            }
            else
            {
                yield return new WaitForSeconds(1f);
                regenTimeout -= 1;
            }
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