using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemChoice : MonoBehaviour
{
    public List<Item> Items;

    public Transform ItemChoiceContent;
    public GameObject ItemChoiceItem;

    private void Start()
    {
        DisplayItemChoices();
    }

    public void DisplayItemChoices()
    {
        Cursor.lockState = CursorLockMode.None;

        List<Item> ItemChoices = Items;
        for (int i = 0; i < 3; i++)
        {
            Item item = ItemChoices[Random.Range(0, ItemChoices.Count)];
            ItemChoices.Remove(item);

            GameObject obj = Instantiate(ItemChoiceItem, ItemChoiceContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImg").GetComponent<Image>();
            var itemDescription = obj.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemDescription.text = item.itemDescription;
        }
    }

    public void Choice()
    {
        Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text);

        gameObject.SetActive(false);
    }
}
