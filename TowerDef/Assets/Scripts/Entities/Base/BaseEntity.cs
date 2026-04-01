using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // Если это враг - даем деньги игроку
        if (gameObject.CompareTag("Enemy"))
        {
            // Обращаемся к менеджеру валюты (нужно будет создать)
            // CurrencyManager.instance.AddMoney(reward);
        }

        // Если это 14 корпус - Game Over
        if (gameObject.name == "Building_14")
        {
            Debug.Log("ГЛАВНЫЙ КОРПУС ПАЛ! ГЕЙМ ОВЕР");
        }

        Destroy(gameObject);
    }
}