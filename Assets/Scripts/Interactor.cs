/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 16/05/2024
 * Date Modified: 19/05/2024
 * Description: Player Interactions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class Interactor : MonoBehaviour
{
    /// <summary>
    /// Raycast Interact Range
    /// </summary>
    public float InteractRange;

    /// <summary>
    /// Interaction UI Prompt
    /// </summary>
    private TextMeshProUGUI interactText;
    private StarterAssetsInputs input;
    private GameManager gameManager;

    private void Awake()
    {
        interactText = GameObject.Find("interactText").GetComponent<TextMeshProUGUI>();
        input = GameObject.Find("PlayerArmature").GetComponent<StarterAssetsInputs>();
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        interactText.enabled = false;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
            {
                interactObj.Interact(gameManager, interactText, input);
            }
            else
            {
                interactText.enabled = false;
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }
}
