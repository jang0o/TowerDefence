using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [Header("Stats from ScriptableObject")]
    public UnitStats stats;

    [Header("Attack Settings")]
    public float attackRange = 0.5f;
    public float attackCooldown = 1.5f;

    protected override void Die()
    {
        if (stats != null)
        {
            CurrencyManager.AddMoney(stats.reward);
            Debug.Log("Enemy killed! Reward: " + stats.reward);
        }
        base.Die();
    }

    protected Waypoints targetPath;
    protected Transform targetPoint;
    protected int pointIndex = 0;

    protected float lastAttackTime;
    protected BaseEntity currentTarget;

    public void SetupPath(Waypoints path)
    {
        targetPath = path;
        pointIndex = 0;
        if (targetPath != null && targetPath.points.Length > 0)
            targetPoint = targetPath.points[0];
    }

    protected virtual void Start()
    {
        if (stats != null)
        {
            maxHealth = stats.health;
            health = maxHealth;
        }
    }

    protected virtual void Update()
    {
        CheckForBuildings();
        if (currentTarget != null)
        {
            AttackTarget();
            return;
        }

        Move();
    }

    protected void CheckForBuildings()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange + 0.1f);
        foreach (var hit in hitColliders)
        {
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

    protected virtual void AttackTarget()
    {
        if (currentTarget == null || currentTarget.health <= 0)
        {
            currentTarget = null;
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            currentTarget.TakeDamage(stats.damage);
            lastAttackTime = Time.time;
            Debug.Log(gameObject.name + " is breaking " + currentTarget.gameObject.name + "! Damage: " + stats.damage);
        }
    }

    protected void Move()
    {
        if (targetPoint == null || stats == null) return;

        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        Vector3 direction = targetPos - transform.position;
        
        if (direction.magnitude < 0.3f)
        {
            GetNextPoint();
            return;
        }

        transform.Translate(direction.normalized * stats.speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    protected void GetNextPoint()
    {
        if (targetPath == null || pointIndex >= targetPath.points.Length - 1)
        {
            ReachEnd();
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    protected void ReachEnd()
    {
        GameObject mainBuilding = GameObject.Find("14k+7k");
        if (mainBuilding != null)
        {
            BaseEntity entity = mainBuilding.GetComponent<BaseEntity>();
            if (entity != null && entity.health > 0)
            {
                currentTarget = entity;
                targetPoint = null;
                Debug.Log(gameObject.name + " reached the final target and is attacking!");
                return;
            }
        }
        
        // Fallback: if target is gone, just stay there or destroy if it's been too long
        // Destroy(gameObject); // Don't destroy if we want them to stay
    }
}
