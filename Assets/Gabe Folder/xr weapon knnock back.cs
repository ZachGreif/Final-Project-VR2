using System.Collections.Generic;
using UnityEngine;

public class XRWeaponKnockback : MonoBehaviour
{
    [Header("Hit Settings")]
    public float minimumSwingSpeed = 0.75f;
    public float bonusForce = 1.0f;
    public float hitCooldown = 0.2f;

    [Header("Velocity Tracking")]
    public bool useLocalSpaceTracking = false;

    private Vector3 lastPosition;
    private Vector3 currentVelocity;

    private Dictionary<EnemyKnockback, float> hitTimers = new Dictionary<EnemyKnockback, float>();

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        currentVelocity = (currentPosition - lastPosition) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = currentPosition;

        if (hitTimers.Count > 0)
        {
            List<EnemyKnockback> expired = new List<EnemyKnockback>();

            foreach (KeyValuePair<EnemyKnockback, float> pair in hitTimers)
            {
                if (Time.time >= pair.Value)
                {
                    expired.Add(pair.Key);
                }
            }

            for (int i = 0; i < expired.Count; i++)
            {
                hitTimers.Remove(expired[i]);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        TryApplyKnockback(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        TryApplyKnockback(collision);
    }

    void TryApplyKnockback(Collision collision)
    {
        EnemyKnockback enemy = collision.collider.GetComponentInParent<EnemyKnockback>();
        if (enemy == null)
        {
            return;
        }

        if (hitTimers.ContainsKey(enemy))
        {
            return;
        }

        Vector3 measuredVelocity = currentVelocity;

        if (useLocalSpaceTracking)
        {
            measuredVelocity = transform.TransformDirection(currentVelocity);
        }

        float swingSpeed = measuredVelocity.magnitude;
        if (swingSpeed < minimumSwingSpeed)
        {
            return;
        }

        Vector3 hitPoint;

        if (collision.contactCount > 0)
        {
            hitPoint = collision.GetContact(0).point;
        }
        else
        {
            hitPoint = collision.transform.position;
        }

        enemy.ApplyKnockback(hitPoint, measuredVelocity, bonusForce);
        hitTimers.Add(enemy, Time.time + hitCooldown);
    }
}