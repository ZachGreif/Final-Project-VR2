using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyKnockback : MonoBehaviour
{
    [Header("Knockback Settings")]
    public float knockbackMultiplier = 1.2f;
    public float upwardForce = 0.75f;
    public float maxKnockbackForce = 8f;

    [Header("Optional")]
    public bool zeroVelocityBeforeHit = false;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 hitPoint, Vector3 attackerVelocity, float bonusForce = 0f)
    {
        Vector3 direction = transform.position - hitPoint;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = -transform.forward;
            direction.y = 0f;
        }

        direction.Normalize();

        float speed = attackerVelocity.magnitude;
        float totalForce = (speed * knockbackMultiplier) + bonusForce;
        totalForce = Mathf.Clamp(totalForce, 0f, maxKnockbackForce);

        Vector3 finalForce = direction * totalForce;
        finalForce.y += upwardForce;

        if (zeroVelocityBeforeHit)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        rb.AddForce(finalForce, ForceMode.Impulse);
    }
}
