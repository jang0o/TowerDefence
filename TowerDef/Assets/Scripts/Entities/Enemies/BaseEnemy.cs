using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public UnitStats stats; // Теперь ошибка CS1061 исчезнет, так как в stats есть damage

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

    void Update()
    {
        if (targetPoint == null) return;

        // ДВИЖЕНИЕ В 3D (по X и Z)
        Vector3 targetPos = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
        Vector3 direction = targetPos - transform.position;

        transform.Translate(direction.normalized * stats.speed * Time.deltaTime, Space.World);

        // ПОВОРОТ
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        // ПРОВЕРКА ДИСТАНЦИИ
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                             new Vector3(targetPos.x, 0, targetPos.z)) < 0.2f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        if (pointIndex >= targetPath.points.Length - 1)
        {
            // Здесь используем тот самый stats.damage, который вызывал ошибку
            Debug.Log("Нанесено урона корпусу: " + stats.damage);
            Destroy(gameObject);
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }
}