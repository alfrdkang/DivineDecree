using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public GameObject[] monsters;
    int randomSpawnPoint, randomMonster;
    public static bool spawnAllowed;

    private void Start()
    {
        spawnAllowed = true;
        InvokeRepeating("SpawnAMonster", 0f, 3f);
    }

    public void SpawnAMonster()
    {
        if (spawnAllowed)
        {
            randomSpawnPoint = Random.Range(0, spawnpoints.Length);
            randomMonster = Random.Range(0, monsters.Length);
            Instantiate(monsters[randomMonster], spawnpoints[randomSpawnPoint].position, Quaternion.identity);
        }
    }
}
