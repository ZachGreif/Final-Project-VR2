using UnityEngine;
using UnityEngine.AI;
using Unity.XR.CoreUtils;

public class FollowPlayer : MonoBehaviour
{
    public float stoppingDistance = 2f;
    public float followRange = 10f;
    public float updateRate = 0.2f;
    public Transform follow;
    private Transform player;
    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
      
        FindPlayer();
        
        if (player == null)
        {
            Debug.LogError("XR Player (Camera) not found!");
        }
    }

    void FindPlayer()
    {
        // Best XR method: XROrigin camera
        XROrigin origin = FindFirstObjectByType<XROrigin>();

        if (origin != null && origin.Camera != null)
        {
            player = origin.Camera.transform;
            return;
        }

        // Fallback: MainCamera
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        if (cam != null)
        {
            player = cam.transform;
        }
    }
   
    void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Optional range check (keeps your followRange useful)
        if (distance > followRange)
            return;

        timer += Time.deltaTime;

        if (timer >= updateRate)
        {
            agent.SetDestination(player.position);
            timer = 0f;
        }
        Debug.Log("Has path: " + agent.hasPath);
        Debug.Log("Velocity: " + agent.velocity);
    }
}