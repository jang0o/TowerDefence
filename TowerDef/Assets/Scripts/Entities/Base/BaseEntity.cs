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
        // ���� ��� ���� - ���� ������ ������
        if (gameObject.CompareTag("Enemy"))
        {
            // ���������� � ��������� ������ (����� ����� �������)
            // CurrencyManager.instance.AddMoney(reward);
        }

        // ���� ��� 14 ������ - Game Over
        if (gameObject.name == "Building_14")
        {
            Debug.Log("������� ������ ���! ���� ����");
        }

        Destroy(gameObject);
    }
}