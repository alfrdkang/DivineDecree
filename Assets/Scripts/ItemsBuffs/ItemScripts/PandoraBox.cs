using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraBox : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerMaxHealth = GameManager.instance.playerMaxHealth / 2;
        StartCoroutine(ThreeItems());

    }

    private IEnumerator ThreeItems()
    {
        ItemChoice.instance.itemChoiceUI.SetActive(true);
        ItemChoice.instance.DisplayItemChoices();
        while (ItemChoice.instance.ChoiceUIActive)
        {
            yield return null;
        }
        ItemChoice.instance.itemChoiceUI.SetActive(true);
        ItemChoice.instance.DisplayItemChoices();
        while (ItemChoice.instance.ChoiceUIActive)
        {
            yield return null;
        }
        ItemChoice.instance.itemChoiceUI.SetActive(true);
        ItemChoice.instance.DisplayItemChoices();
    }
}
