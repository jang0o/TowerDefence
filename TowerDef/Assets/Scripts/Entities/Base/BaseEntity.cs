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
        if (gameObject.CompareTag("Enemy"))
        {
            // Тут будет начисление валюты
        }

        if (gameObject.name == "14k+7k")
        {
            Debug.Log("ГЛАВНЫЙ КОРПУС ПАЛ! ГЕЙМ ОВЕР");
            if (MenuManager.Instance != null)
            {
                MenuManager.Instance.TriggerGameOver();
            }
        }

        Destroy(gameObject);
    }
}
