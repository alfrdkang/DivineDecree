using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStormPlus : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.playerSkillDamageMultiplier += (GameManager.instance.playerSkillDamageMultiplier / 2);
    }
}
