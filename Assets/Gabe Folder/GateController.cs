using UnityEngine;
using UnityEngine.AI;

public class GateController : MonoBehaviour
{
    public float openHeight = 5f;
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;

    private NavMeshObstacle obstacle;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openHeight;

        obstacle = GetComponent<NavMeshObstacle>();
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPosition,
                speed * Time.deltaTime
            );

            // Disable obstacle when mostly open
            if (Vector3.Distance(transform.position, openPosition) < 0.1f)
            {
                obstacle.enabled = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                closedPosition,
                speed * Time.deltaTime
            );

            // Enable obstacle when closed
            if (Vector3.Distance(transform.position, closedPosition) < 0.1f)
            {
                obstacle.enabled = true;
            }
        }
    }

    public void OpenGate()
    {
        isOpening = true;
    }

    public void CloseGate()
    {
        isOpening = false;
    }
}