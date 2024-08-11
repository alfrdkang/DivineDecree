using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhimsyBubble : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.BubbleShield();
    }
}
