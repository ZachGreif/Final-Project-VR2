using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private EnemyTracker m_EnemyTracker;
    [Header("Stats")]
    public int maxHP = 100;
    public int damage = 10;

    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    // Call this when the monster takes damage
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        Debug.Log(gameObject.name + " took " + amount + " damage. HP: " + currentHP);

        if (currentHP <= 0)
        {
           m_EnemyTracker = GetComponent<EnemyTracker>();
            m_EnemyTracker.Die();

        }
    }

    // Call this when the monster hits something (like player)
    public int GetDamage()
    {
        return damage;
    }
}