using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoopyBirthdayPie : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerHealthRegenerationPerSecond *= (GameManager.instance.playerHealthRegenerationPerSecond / 2);
    }
}
