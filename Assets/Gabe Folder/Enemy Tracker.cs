using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    private SpawnController spawner;

    public void SetSpawner(SpawnController s)
    {
        spawner = s;
    }

    public void Die()
    {
        // Two die functions pick one and get rid of the other at some point

        spawner?.OnEnemyDeath();
        Destroy(gameObject);
    }
}