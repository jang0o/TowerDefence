using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public UnitStats stats;

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;

    public void SetupPath(Waypoints path)
    {
        targetPath = path;
        pointIndex = 0;
        if (targetPath.points.Length > 0)
            targetPoint = targetPath.points[0];
    }

    void Start()
    {
        // 1. Proveryaem stats
        if (stats == null)
        {
            stats = new UnitStats();
            Debug.Log("Stats bily pustie, sozdal de-fultnie dlya " + gameObject.name);
        }
    } // VOT ETOY SKOBKI NE HVATALO!

    void Update()
    {
        if (targetPoint == null || stats == null) return;

        // Dvizhenie k tochke
        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        Vector3 direction = targetPos - transform.position;

        transform.Translate(direction.normalized * stats.speed * Time.deltaTime, Space.World);

        // Povorot
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        // Proverka distancii
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
            Debug.Log("Vrag doshel do konca. Uron baze: " + stats.damage);
            Destroy(gameObject);
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    public void TakeDamage(float amount)
    {
        if (stats == null) return;

        stats.health -= amount;
        Debug.Log("U vraga ostalos HP: " + stats.health);

        if (stats.health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}