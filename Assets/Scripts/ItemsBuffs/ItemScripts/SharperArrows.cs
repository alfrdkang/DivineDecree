using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharperArrows : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerBaseDamage *= 1.2f;
    }
}
