using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MapleSwifties : ItemScript
{
    private void Start()
    {
        ThirdPersonController.instance.MoveSpeed *= 1.2f;
        ThirdPersonController.instance.SprintSpeed *= 1.2f;
    }
}
