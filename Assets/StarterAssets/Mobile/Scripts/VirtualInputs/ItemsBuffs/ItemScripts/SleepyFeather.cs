using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SleepyFeather : ItemScript
{
    private void Start()
    {
        ThirdPersonController.instance._jumpsRemaining += 1;
    }
}
