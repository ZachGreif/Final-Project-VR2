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

        // Check for specific weapon tags (case-insensitive comparison)
        string tag = other.tag.ToLower();
        if (tag == "weapon1")
            m_stat.TakeDamage(5);
        else if (tag == "weapon2")
            m_stat.TakeDamage(10);
        else if (tag == "weapon3")
            m_stat.TakeDamage(8);
        else
            return; // Not a weapon we care about

        Debug.Log(gameObject.name + " hit by " + other.name + " with tag: " + other.tag);

        // Start cooldown
        StartCoroutine(DisableColliderTemporarily());
    }

    IEnumerator DisableColliderTemporarily()
    {
        isOnCooldown = true;
        // Don't disable the collider — just block damage via the flag
        // so the enemy still physically interacts with the world
        yield return new WaitForSeconds(disableTime);
        isOnCooldown = false;
    }
}