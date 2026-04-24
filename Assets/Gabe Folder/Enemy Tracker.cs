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
        spawner?.OnEnemyDeath();

        // Disable NavMeshAgent and colliders immediately so the corpse
        // doesn't block pathfinding or take more hits, then destroy after
        // the death animation has time to play (~2 seconds)
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false;

        Destroy(gameObject, 2f);
    }
}