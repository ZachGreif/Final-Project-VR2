using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    [Header("gates")]
    public GateController Gate;
    public GateController largeGate;
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
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

            // Check if we can spawn more enemies
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemies();
            }
        }
    }

    void SpawnEnemies()
    {
        int spawnAmount = Mathf.Min(spawnBatchSize, maxEnemies - currentEnemyCount);

        for (int i = 0; i < spawnAmount; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            currentEnemyCount++;

            // Hook into enemy death so we can track removal
            EnemyTracker tracker = enemy.GetComponent<EnemyTracker>();
            if (tracker == null)
                tracker = enemy.AddComponent<EnemyTracker>();
            tracker.SetSpawner(this);
        }
    }

    // Called by enemies when they die
    public void OnEnemyDeath()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }
}