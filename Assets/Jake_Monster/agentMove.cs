using UnityEngine;
using UnityEngine.AI;

public class AgentFollowPlayer : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Wandering Settings")]
    public float wanderRadius = 10f;
    public float wanderInterval = 5f;
    private float wanderTimer;
    public bool wander;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        wander = true;
        wanderTimer = 0;
    }

    void Update()
    {
        if (!wander)
            chasePlayer();
        else
            WanderAround();

        // Drive movement animation
        if (anim != null)
            anim.SetFloat("Speed", agent.velocity.magnitude);
    }
    void WanderAround()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, NavMesh.AllAreas);
            if (newPos != transform.position)
                agent.SetDestination(newPos);
            wanderTimer = 0;
        }
    }

    public void StartChase()
    {
        if (wander)
        {
            print("Started Chase");
            agent.speed = 3.5f;
            agent.ResetPath();
            wander = false;
        }
    }
    public void StopChase() 
    {
        if (!wander)
        {
            print("Ended Chase");
            agent.speed = 2f;
            agent.ResetPath();
            wander = true;
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection.y = 0;
        randomDirection += origin;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, dist, layermask))
            return navHit.position;

        return origin;
    }

    void chasePlayer()
    {
        if (player == null || agent == null)
            return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 2f, NavMesh.AllAreas))
            agent.SetDestination(player.position);
    }
}
