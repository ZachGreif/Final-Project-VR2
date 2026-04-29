using UnityEngine;
using UnityEngine.AI;
using Unity.XR.CoreUtils; // Needed for XROrigin

public class FollowPlayer : MonoBehaviour
{
    public float stoppingDistance = 2f;
    public float followRange = 10f;

    private Transform player; // No longer public

    private NavMeshAgent agent;
    public float updateRate = 0.2f;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;

        // Find XR Origin in the scene
        XROrigin xrOrigin = FindFirstObjectByType<XROrigin>();

        if (xrOrigin != null)
        {
            player = xrOrigin.transform;
        }
        else
        {
            Debug.LogError("XR Origin not found in scene!");
        }
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
    }
}