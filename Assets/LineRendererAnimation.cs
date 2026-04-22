using UnityEngine;

using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class LineRendererAnimator : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [Header("Animation Settings")]
    [Tooltip("How long it takes to move from Z:12 to Z:20")]
    public float animationDuration = 1.5f;

    private float startZ = 12f;
    private float endZ = 20f;
    private int targetIndex = 1;

    public bool play = false;

    public BoltLauncher boltLauncher;

    private Vector3 boltSpawnPosition;Vector3 boltSpawnRotation;


    private void Awake()
    {
        // Grab the Line Renderer attached to this GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Safety check to ensure there are enough points on the line
        if (lineRenderer.positionCount < 2)
        {
            lineRenderer.positionCount = 2;
        }

        if (boltLauncher != null)
        {
            boltSpawnPosition = new Vector3(0,2.81f,6.23f);
            boltSpawnRotation = boltLauncher.transform.localEulerAngles;
        }   
    }

    private void Update()
    {
        if (play)
        {
            PlayLineAnimation();
            play = false;
        }
    }



    /// <summary>
    /// Call this method from a button, trigger, or other script to start the animation.
    /// </summary>
    public void PlayLineAnimation()
    {
        // Stop any currently running animations on this script so they don't overlap
        StopAllCoroutines();
        StartCoroutine(AnimateLinePoint());
    }

    private IEnumerator AnimateLinePoint()
    {
        // Get the current position of index 1 so we retain its X and Y coordinates
        Vector3 pointPos = lineRenderer.GetPosition(targetIndex);
        float elapsedTime = 0f;

        // 1. Slowly animate Z from 12 to 20
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the percentage of completion (0 to 1)
            float t = elapsedTime / animationDuration;

            // Interpolate the Z value based on time
            pointPos.z = Mathf.Lerp(startZ, endZ, t);

            // Apply the new position to the Line Renderer
            lineRenderer.SetPosition(targetIndex, pointPos);

            // Wait until the next frame before continuing the loop
            yield return null;
        }

        // 2. Snap back to Z = 12
        pointPos.z = startZ;
        lineRenderer.SetPosition(targetIndex, pointPos);
        if (boltLauncher != null)
        {
            boltLauncher.Launch();
            GameObject bolt = Instantiate(boltLauncher.gameObject, boltSpawnPosition, Quaternion.Euler(boltSpawnRotation), this.gameObject.transform);
            bolt.transform.localPosition = boltSpawnPosition; ;
            boltLauncher = bolt.GetComponent<BoltLauncher>();
        }
    }
}