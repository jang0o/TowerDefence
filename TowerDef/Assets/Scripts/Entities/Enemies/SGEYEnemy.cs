using UnityEngine;

public class SGEYEnemy : BaseEnemy
{
    [Header("SGEY Special")]
    public int moneyStealAmount = 5;

    protected override void AttackTarget()
    {
        if (currentTarget == null || currentTarget.health <= 0)
        {
            currentTarget = null;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            currentTarget.TakeDamage(stats.damage);
            
            CurrencyManager.SubtractMoney(moneyStealAmount);
            
            lastAttackTime = Time.time;
            Debug.Log(gameObject.name + " attacked and stole money!");
        }
    }
}
