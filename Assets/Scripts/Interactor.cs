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

    private bool interactableHit;

    private void Awake()
    {
        interactText = GameObject.Find("interactText").GetComponent<TextMeshProUGUI>();
        interactText.enabled = false;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out Interactable interactObj))
            {
                interactableHit = true;
                Outline obj = hitInfo.collider.gameObject.GetComponent<Outline>();
                obj.enabled = true;

                StartCoroutine(outlineManager(obj));
                interactObj.Interact(gameManager, interactText);
            }
            else
            {
                interactableHit = false;
                interactText.enabled = false;
                StarterAssetsInputs.instance.interact = false;
            }
        }
        else
        {
            interactText.enabled = false;
        }
    }

    private IEnumerator outlineManager(Outline obj) { 
        while (interactableHit) 
        {
            yield return null;
        }
        obj.enabled = false;
    }
}
