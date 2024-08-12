using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    private Rigidbody rb;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        float speed = 30f;
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.tag == "TriggerArea"))
        {
            if (other.tag == "Enemy")
            {
                if (other.gameObject.TryGetComponent(out EnemyAI enemyAI))
                {
                    other.gameObject.GetComponent<EnemyAI>().TakeDamage((int)damage);
                }
                else if (other.gameObject.TryGetComponent(out RangedEnemyAI rangedEnemyAI))
                {
                    other.gameObject.GetComponent<RangedEnemyAI>().TakeDamage((int)damage);
                }

                Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            }
            else if (other.tag == "Boss")
            {
                if (other.gameObject.GetComponent<TreeBoss>() != null)
                {
                    other.gameObject.GetComponent<TreeBoss>().TakeDamage((int)damage);
                }
                else if (other.gameObject.GetComponent<BurrowBoss>() != null)
                {
                    Debug.Log("damage?");
                    other.gameObject.GetComponent<BurrowBoss>().TakeDamage((int)damage);
                }
                else if (other.gameObject.GetComponent<BullBoss>() != null)
                {
                    other.gameObject.GetComponent<BullBoss>().TakeDamage((int)damage);
                }

                Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            }
            else if (other.tag == "Crate")
            {
                ItemChoice.instance.itemChoiceUI.SetActive(true);
                ItemChoice.instance.DisplayItemChoices();
                Destroy(other.gameObject);
                Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}