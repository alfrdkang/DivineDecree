using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStormAOE : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Enemy"))
        {
            StartCoroutine(ApplyDamageOverTime(other));
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
            
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Enemy"))
        {
            StopCoroutine(ApplyDamageOverTime(other));
        }
    }

    private IEnumerator ApplyDamageOverTime(Collider enemy)
    {
        while (enemy != null && enemy.CompareTag("Enemy"))
        {
            enemy.GetComponent<EnemyAI>().TakeDamage((int)(GameManager.instance.playerBaseDamage * GameManager.instance.playerSkillDamageMultiplier));
            yield return new WaitForSeconds(1f);
        }
    }
}

