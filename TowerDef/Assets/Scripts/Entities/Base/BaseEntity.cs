using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    [Header("Общие характеристики")]
    public float health;
    public float maxHealth;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    protected virtual void Die()
    {
        // Логика смерти (деньги, эффекты)
        if (gameObject.CompareTag("Enemy"))
        {
            // Тут будет начисление валюты
        }

        if (gameObject.name == "Building_14")
        {
            Debug.Log("ГЛАВНЫЙ КОРПУС ПАЛ! ГЕЙМ ОВЕР");
        }

        Destroy(gameObject);
    }
}