using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private EnemyTracker m_EnemyTracker;
    private Animator anim;

    [Header("Stats")]
    public int maxHP = 100;
    public int damage = 10;

    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        anim = GetComponentInChildren<Animator>();
    }

    // Call this when the monster takes damage
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHP);

        if (anim != null) anim.SetTrigger("Hit");

        if (currentHP <= 0)
        {
            if (anim != null) anim.SetTrigger("Die");
            m_EnemyTracker = GetComponent<EnemyTracker>();
            if (m_EnemyTracker != null)
                m_EnemyTracker.Die();
            else
            {
                Debug.LogError(gameObject.name + " has no EnemyTracker — destroying directly.");
                Destroy(gameObject);
            }
        }
    }

    // Call this when the monster hits something (like player)
    public int GetDamage()
    {
        return damage;
    }
}