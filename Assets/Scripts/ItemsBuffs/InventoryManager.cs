using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public GameObject inventoryUI;
    private bool inventoryUIActive;

    private void Awake()
    {
        instance = this;
        inventoryUI.SetActive(false);
        inventoryUIActive = true;
    }

    private void Update()
    {
        if (StarterAssetsInputs.instance.inventory)
        {
            ListItems();
            if (inventoryUIActive)
            {
                inventoryUI.SetActive(false);
                inventoryUIActive = false;
            } else
            {
                inventoryUI.SetActive(true);
                inventoryUIActive = true;
            }
            StarterAssetsInputs.instance.inventory = false;
        }
    }

    public void Add(Item item)
    {
        Items.Add(item);
        //Instantiate(item.itemObj);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
        Destroy(item.itemObj);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemImg").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }
}
