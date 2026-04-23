using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public float stoppingDistance = 2f;
    public float followRange = 10f;
    public Transform player;          // Assign the player in Inspector
    private NavMeshAgent agent;

    public float updateRate = 0.2f;   // How often to update destination

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;


        if (player == null)
        {
            Debug.LogError("Player not assigned!");
        }
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;

        // Update destination at intervals for performance
        if (timer >= updateRate)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }

    }
}