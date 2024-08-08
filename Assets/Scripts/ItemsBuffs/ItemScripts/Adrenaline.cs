using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class Adrenaline : ItemScript
{
    private void Start()
    {
        ThirdPersonShooter.instance.shootCD *= 0.8f;
    }
}
