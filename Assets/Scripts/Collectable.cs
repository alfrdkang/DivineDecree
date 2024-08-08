/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 30/04/2024
 * Date Modified: 16/05/2024
 * Description: Collectables (Key, Artifacts, Coins)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    /// <summary>
    /// Coin Rotation Speed
    /// </summary>
    public float coinRotateSpeed = 250.0f;

    /// <summary>
    /// Player Interaction Function
    /// </summary>
    public void Interact()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Player Collision Function
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && gameObject.tag == "Coin")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (gameObject.tag == "Coin")
        {
            float Rotate = coinRotateSpeed * Time.deltaTime;
            transform.Rotate(Rotate, 0, 0);
        }
    }
}
