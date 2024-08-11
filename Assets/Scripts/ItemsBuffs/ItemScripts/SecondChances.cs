using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondChances : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.extraLives += 1;
        ItemChoice.instance.cursedItems = false;
    }
}
