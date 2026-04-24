using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    [Header("gates")]
    public GateController Gate;
    public GateController largeGate;

    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;

    [Header("Limits")]
    public int maxEnemies = 10;
    public int spawnBatchSize = 3;

    [Header("Timing")]
    public float checkInterval = 5f;

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        int spawnAmount = Mathf.Min(spawnBatchSize, maxEnemies - currentEnemyCount);

        for (int i = 0; i < spawnAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

            currentEnemyCount++;

            EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
            tracker.SetSpawner(this);
        }
    }

    // Called by enemies when they die
    public void OnEnemyDeath()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}