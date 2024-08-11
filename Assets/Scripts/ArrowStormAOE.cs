using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStormAOE : MonoBehaviour
{

    [SerializeField] private GameObject boomText;
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
            if (enemy.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
            {
                enemyAI.TakeDamage((int)(GameManager.instance.playerBaseDamage * GameManager.instance.playerSkillDamageMultiplier));
            } else if (enemy.TryGetComponent<RangedEnemyAI>(out RangedEnemyAI rangedEnemyAI))
            {
                rangedEnemyAI.TakeDamage((int)(GameManager.instance.playerBaseDamage * GameManager.instance.playerSkillDamageMultiplier));
            }
            GameObject obj = Instantiate(boomText, enemy.transform.position, enemy.transform.rotation);
            yield return new WaitForSeconds(1f);
            Destroy(obj);
        }
    }
}

