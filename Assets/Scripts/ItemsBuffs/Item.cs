using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite icon;
    public GameObject itemObj;
    public bool cursed;
    public int count = 1;
}
