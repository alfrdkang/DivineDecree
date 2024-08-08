using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;
using Random = UnityEngine.Random;

public class ItemChoice : MonoBehaviour
{
    public List<Item> Items;

    public Transform ItemChoiceContent;
    public GameObject ItemChoiceItem;

    public static ItemChoice instance;
    public GameObject itemChoiceUI;
    private bool ChoiceUIActive;

    [SerializeField] private GameObject HUD; // Reference to the in-game HUD UI

    private void Awake()
    {
        instance = this;
        itemChoiceUI.SetActive(false);
        ChoiceUIActive = true;
    }

    public void DisplayItemChoices()
    {
        Time.timeScale = 0f; // Stop time
        HUD.SetActive(false); // Disable in-game HUD UI
        StarterAssetsInputs.instance.inputs = false;
        Cursor.lockState = CursorLockMode.None;

        List<Item> ItemChoices = new List<Item>(Items);

        for (int i = 0; i < 3; i++)
        {
            Item item = ItemChoices[Random.Range(0, ItemChoices.Count)];

            GameObject obj = Instantiate(ItemChoiceItem, ItemChoiceContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImg").GetComponent<Image>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();

            obj.GetComponent<Button>().onClick.AddListener(Choice);

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemDescription.text = item.itemDescription;

            ItemChoices.Remove(item);
        }
    }

    public void Choice()
    {
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text);

        foreach (Transform child in ItemChoiceContent)
        {
            Destroy(child.gameObject);
        }

        ItemChoice.instance.itemChoiceUI.SetActive(false);
        StarterAssetsInputs.instance.inputs = true;
        Time.timeScale = 1f; // Resumes time
        ItemChoice.instance.HUD.SetActive(true); // Enable in-game HUD UI
        Cursor.lockState = CursorLockMode.Locked;
    }
}
