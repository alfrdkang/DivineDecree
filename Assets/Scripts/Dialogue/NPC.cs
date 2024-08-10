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
    public Dialogue dialogue;

    private void Awake()
    {
        gameObject.GetComponent<Outline>().enabled = false;
    }

    public override void Interact(GameManager gameManager, TextMeshProUGUI interactText)
    {
        base.Interact(gameManager, interactText);
        interactText.text = "[E] Talk to " + gameObject.name;

        // Check if interact key is pressed
        if (StarterAssetsInputs.instance.interact)
        {
            diagVirtualCamera.Follow = transform.Find("DiagCameraAim").transform;
            TriggerDialogue(dialogue);
            StarterAssetsInputs.instance.interact = false; // Reset interact input
        }
    }
}
