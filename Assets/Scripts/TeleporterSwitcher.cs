using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleporterSwitcher : MonoBehaviour
{
    [SerializeField] private PlayerTeleporter playerTeleporter;
    public Vector3 newPosition;

    private void OnTriggerEnter(Collider other)
    {
        playerTeleporter.startPosition = newPosition;
    }
}