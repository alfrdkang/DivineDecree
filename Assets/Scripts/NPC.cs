/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 31/07/2024
 * Date Modified: 
 * Description: NPC Interact Functions
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
using Cinemachine;

public class NPC : Interactable
{
    public CinemachineVirtualCamera diagVirtualCamera;
    public Dialogue sageDialogue;

    /// <param name="gameManager">The GameManager instance managing the game.</param>
    /// <param name="interactText">The UI text to display interaction prompts.</param>
    /// <param name="_input">The input handler for player controls.</param>
    public override void Interact(GameManager gameManager, TextMeshProUGUI interactText, StarterAssetsInputs _input)
    {
        base.Interact(gameManager, interactText, _input);
        interactText.text = "[E] Talk to " + gameObject.name;

        // Check if interact key is pressed
        if (_input.interact)
        {
            diagVirtualCamera.Follow = transform;
            TriggerDialogue(sageDialogue);
            _input.interact = false; // Reset interact input
        }
    }
}
