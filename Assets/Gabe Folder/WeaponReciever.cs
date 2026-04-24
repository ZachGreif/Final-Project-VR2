using UnityEngine;
using System.Collections;

public class WeaponHitReceiver : MonoBehaviour
{
    private Monster m_stat;

    [Header("Cooldown")]
    public float disableTime = 1f;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip[] weapon1Sounds;
    public AudioClip[] weapon2Sounds;
    public AudioClip[] weapon3Sounds;

    private Collider col;
    private bool isOnCooldown = false;

    void Start()
    {
        col = GetComponent<Collider>();
        m_stat = GetComponent<Monster>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
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

        int damage = 0;
        AudioClip[] soundPool = null;
        bool validHit = false;

        // ??? Weapon 1
        if (other.CompareTag("weapon1"))
        {
            damage = 5;
            soundPool = weapon1Sounds;
            validHit = true;
        }
        // ??? Weapon 2
        else if (other.CompareTag("weapon2"))
        {
            damage = 10;
            soundPool = weapon2Sounds;
            validHit = true;
        }
        // ??? Weapon 3
        else if (other.CompareTag("weapon3"))
        {
            damage = 8;
            soundPool = weapon3Sounds;
            validHit = true;
        }

        // ? ignore non-weapons
        if (!validHit) return;

        // ?? Apply damage
        if (m_stat != null)
        {
            m_stat.TakeDamage(damage);
        }

        // ?? Play random weapon sound
        if (soundPool != null && soundPool.Length > 0 && audioSource != null)
        {
            AudioClip clip = soundPool[Random.Range(0, soundPool.Length)];
            audioSource.PlayOneShot(clip);
        }

        Debug.Log(gameObject.name + " hit by " + other.name + " (" + other.tag + ")");

        StartCoroutine(DisableColliderTemporarily());
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