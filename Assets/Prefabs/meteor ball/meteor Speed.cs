using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpeed : MonoBehaviour
{
    public float meteorSpeed1;
    public GameObject impactPrefab;
    private Rigidbody rb;
    public List<GameObject> trails;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (impactPrefab == null)
        {
            Debug.LogError("impactPrefab is not assigned in the Inspector.");
        }
        else
        {
            Debug.Log("impactPrefab assigned correctly.");
        }
    }

    void FixedUpdate()
    {
        if (meteorSpeed1 != 0 && rb != null)
        {
            rb.position += transform.forward * (meteorSpeed1 * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        meteorSpeed1 = 0;

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (impactPrefab != null)
        {
            Debug.Log("Instantiating impact prefab at position: " + pos + " with rotation: " + rot);
            var impactVFX = Instantiate(impactPrefab, pos, rot) as GameObject;
            Destroy(impactVFX, 5f);
        }
        else
        {
            Debug.LogError("Impact prefab is null on collision!");
        }

        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                }
            }
        }
        Destroy(gameObject);
    }
}
