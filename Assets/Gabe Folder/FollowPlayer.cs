using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public float stoppingDistance = 2f;
    public float followRange = 10f;
    public Transform player;
    private NavMeshAgent agent;
    private Animator anim;

    public float updateRate = 0.2f;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        anim = GetComponentInChildren<Animator>();

        if (player == null)
            Debug.LogError("Player not assigned!");
    }

    void Update()
    {
        if (player == null) return;

        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }

        // Drive walk/idle animation based on agent speed
        if (anim != null)
            anim.SetFloat("Speed", agent.velocity.magnitude);
    }
}