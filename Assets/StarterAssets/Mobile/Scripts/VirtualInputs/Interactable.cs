/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 31/07/24
 * Date Modified: 
 * Description: Interactable Class
 */

using UnityEngine;
using TMPro;
using StarterAssets;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(GameManager gameManager, TextMeshProUGUI interactText, StarterAssetsInputs input)
    {
        if (!FindObjectOfType<DialogueManager>().diagActive)
        {
            interactText.enabled = true;
        }
    }

    /// <summary>
    /// Function to trigger and start dialogue
    /// </summary>
    /// <param name="Diag"></param>
    public void TriggerDialogue(Dialogue Diag)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(Diag);
    }
}

