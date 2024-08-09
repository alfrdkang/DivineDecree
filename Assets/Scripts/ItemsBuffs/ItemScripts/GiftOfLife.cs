using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftOfLife : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerMaxHealth += 20;
    }
}
