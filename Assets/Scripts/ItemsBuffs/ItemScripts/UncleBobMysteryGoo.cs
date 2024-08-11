using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class UncleBobMysteryGoo : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerBaseDamage *= 1.5f;
        ThirdPersonController.instance.MoveSpeed *= 0.7f;
        ThirdPersonController.instance.SprintSpeed *= 0.7f;
    }
}
