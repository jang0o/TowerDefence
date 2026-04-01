using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public enum EnemyType { Tank, Destroyer } // Танк идет к 14, Разрушитель бьет всё

    [Header("Тип поведения")]
    public EnemyType behavior;

    [Header("Характеристики")]
    public UnitStats stats;
    public float attackCooldown = 1f;

    private Transform targetBuilding;
    private float nextAttackTime;
    private bool isAttacking = false;

    void Update()
    {
        if (isAttacking)
        {
            if (targetBuilding == null)
            {
                isAttacking = false;
                return;
            }

            // Логика удара (ближний бой)
            if (Time.time >= nextAttackTime)
            {
                Attack(targetBuilding);
                nextAttackTime = Time.time + attackCooldown;
            }
            return; // Если атакуем, то не идем дальше
        }

        if (behavior == EnemyType.Destroyer)
        {
            CheckForBuildings();
        }

        MoveTowardsMainBase();
    }

    void CheckForBuildings()
    {
        // Поиск ближайшего здания в небольшом радиусе перед собой
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // Радиус агрессии
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Building")) // Зданиям (корпусам) нужно дать тег Building
            {
                targetBuilding = hit.transform;
                isAttacking = true;
                break;
            }
        }
    }

    void MoveTowardsMainBase()
    {
        // Здесь твоя логика движения по Waypoints (точкам)
        // Если это Танк, он просто игнорирует CheckForBuildings и всегда выполняет этот метод
    }

    void Attack(Transform building)
    {
        // Наносим урон зданию
        BaseEntity b = building.GetComponent<BaseEntity>();
        if (b != null)
        {
            b.TakeDamage(stats.damage);
            Debug.Log(gameObject.name + " ударил здание!");
        }
    }
}