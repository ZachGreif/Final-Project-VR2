using UnityEngine;
using System.Collections;

public class WeaponHitReceiver : MonoBehaviour
{
    private Monster m_stat;
    public float disableTime = 1f;

    private Collider col;
    private bool isOnCooldown = false;

    void Start()
    {
        col = GetComponent<Collider>();
        m_stat = GetComponent<Monster>();
    }

    void OnCollisionEnter(Collision collision)
    {
        TryHandleHit(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        TryHandleHit(other.gameObject);
    }

    void TryHandleHit(GameObject other)
    {
        if (isOnCooldown) return;

        // Check for specific weapon tags
        if (other.CompareTag("weapon1"))
           { m_stat.TakeDamage(5); }
        else if (other.CompareTag("weapon2"))
            { m_stat.TakeDamage(10); }
        else if (other.CompareTag("weapon3"))
                { m_stat.TakeDamage(8); }
        {
            Debug.Log(gameObject.name + " hit by " + other.name + " with tag: " + other.tag);

            // Start cooldown
            StartCoroutine(DisableColliderTemporarily());
        }
    }

    IEnumerator DisableColliderTemporarily()
    {
        isOnCooldown = true;
        col.enabled = false;

        yield return new WaitForSeconds(disableTime);

        col.enabled = true;
        isOnCooldown = false;
    }
}