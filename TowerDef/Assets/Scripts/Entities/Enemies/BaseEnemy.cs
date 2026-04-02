using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [Header("????????? ?? ScriptableObject")]
    public UnitStats stats;

    [Header("????????? ?????")]
    public float attackRange = 1.0f; // ?????????, ?? ??????? ?? ?????? ???? ??????
    public float attackCooldown = 1.5f; // ????? ????? ??????? (? ????????)

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;

    private float lastAttackTime;
    private BaseEntity currentTarget; // ??????? ??????, ??????? ?? ????

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
        // 1. ?????????, ??? ?? ?????? ????? ????
        CheckForBuildings();

        // 2. ???? ?? ????-?? ???? — ?? ?????????
        if (currentTarget != null)
        {
            AttackTarget();
            return;
        }

        // 3. ???? ???? ??? — ?????????? ????
        Move();
    }

    void CheckForBuildings()
    {
        // ???? ?????????? ? ????????? ??????? ????? ?????
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hitColliders)
        {
            // ???? ????? ?????? ? ????? Building ? ?? ??? ???? ?????? BaseEntity
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
        // ???? ?????? ??? ????????? — ?????????? ???? ? ???? ??????
        if (currentTarget == null || currentTarget.health <= 0)
        {
            currentTarget = null;
            return;
        }

        // ?????? ?????
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            currentTarget.TakeDamage(stats.damage);
            lastAttackTime = Time.time;
            Debug.Log(gameObject.name + " ?????? ??????! ????: " + stats.damage);
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
        // ?????? ????? ?????????? 14-?? ???????
        GameObject mainBuilding = GameObject.Find("Building_14");
        if (mainBuilding != null)
        {
            mainBuilding.GetComponent<BaseEntity>().TakeDamage(stats.damage);
        }
        Destroy(gameObject);
    }
}