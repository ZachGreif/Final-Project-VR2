using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class BoltLauncher : MonoBehaviour
{
    [Header("Launch Settings")]
    [Tooltip("The speed at which the Glad Bolt is propelled forward.")]
    public float launchSpeed = 20f;

    [Tooltip("Reference to the LineRendererAnimator that triggers the launch.")]
    public LineRendererAnimator lineRendererAnimator;

    private Rigidbody rb;
    private BoxCollider boxCollider;

    private void Awake()
    {
        // Cache the components at the start so they are ready when we need them
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        // Auto-find the LineRendererAnimator if not assigned
        if (lineRendererAnimator == null)
        {
            lineRendererAnimator = GetComponent<LineRendererAnimator>();
        }
    }

    private void Update()
    {
        // Listen for when the animation plays and trigger launch when it completes
        if (lineRendererAnimator != null && lineRendererAnimator.play)
        {
            // Schedule the launch to occur after the animation duration
            Invoke(nameof(Launch), lineRendererAnimator.animationDuration);
        }
    }

    /// <summary>
    /// Call this method to disable kinematics, turn off the collider, and launch the bolt.
    /// </summary>
    public void Launch()
    {
        Debug.Log("Launch() called! Velocity set to: " + (transform.forward * launchSpeed));

        // 1. Uncheck Is Kinematic so the physics engine takes control
        rb.isKinematic = false;

        // 2. Turn off the Box Collider
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        // 3. Send the object forward at the chosen speed
        // Note: transform.forward pushes the object in the direction its local Z-axis (the blue arrow) is pointing
        rb.linearVelocity = transform.forward * launchSpeed;

        // 4. Detach the Glad Bolt from its parent (the Glad Crossbow)
        transform.SetParent(null);
    }
}