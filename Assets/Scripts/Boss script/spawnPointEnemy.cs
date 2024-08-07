using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPointEnemy : MonoBehaviour

{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 5f; // Time between spawns
    public int maxEnemies = 10; // Maximum number of enemies to spawn

    private int currentEnemyCount = 0; // Current number of enemies spawned

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            currentEnemyCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy at the chosen spawn point
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Optionally, you can adjust the maximum number of enemies here
        // or handle any additional logic related to spawning.
    }
}


