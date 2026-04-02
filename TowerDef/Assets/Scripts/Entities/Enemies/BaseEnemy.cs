using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [Header("Настройки из ScriptableObject")]
    public UnitStats stats;

    [Header("Настройки атаки")]
    public float attackRange = 1.0f; // Дистанция, на которой он начнет бить здание
    public float attackCooldown = 1.5f; // Пауза между ударами (в секундах)

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;

    private float lastAttackTime;
    private BaseEntity currentTarget; // Текущее здание, которое мы бьем

    public void SetupPath(Waypoints path)
    {
        targetPath = path;
        pointIndex = 0;
        if (targetPath != null && targetPath.points.Length > 0)
            targetPoint = targetPath.points[0];
    }

    void Start()
    {
        if (stats != null)
        {
            maxHealth = stats.health;
            health = maxHealth;
        }
    }

    void Update()
    {
        // 1. Проверяем, нет ли здания перед нами
        CheckForBuildings();

        // 2. Если мы кого-то бьем — не двигаемся
        if (currentTarget != null)
        {
            AttackTarget();
            return;
        }

        // 3. Если цели нет — продолжаем путь
        Move();
    }

    void CheckForBuildings()
    {
        // Ищем коллайдеры в небольшом радиусе перед собой
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitColliders)
        {
            // Если нашли объект с тегом Building и на нем есть скрипт BaseEntity
            if (hit.CompareTag("Building"))
            {
                BaseEntity building = hit.GetComponent<BaseEntity>();
                if (building != null && building.health > 0)
                {
                    currentTarget = building;
                    return;
                }
            }
        }
    }

    void AttackTarget()
    {
        // Если здание уже разрушено — сбрасываем цель и идем дальше
        if (currentTarget == null || currentTarget.health <= 0)
        {
            currentTarget = null;
            return;
        }

        // Таймер атаки
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            currentTarget.TakeDamage(stats.damage);
            lastAttackTime = Time.time;
            Debug.Log(gameObject.name + " ударил здание! Урон: " + stats.damage);
        }
    }

    void Move()
    {
        if (targetPoint == null || stats == null) return;

        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        Vector3 direction = targetPos - transform.position;
        transform.Translate(direction.normalized * stats.speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(targetPos.x, 0, targetPos.z)) < 0.2f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        if (targetPath == null || pointIndex >= targetPath.points.Length - 1)
        {
            ReachEnd();
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    void ReachEnd()
    {
        // Логика урона финальному 14-му корпусу
        GameObject mainBuilding = GameObject.Find("Building_14");
        if (mainBuilding != null)
        {
            mainBuilding.GetComponent<BaseEntity>().TakeDamage(stats.damage);
        }
        Destroy(gameObject);
    }
}