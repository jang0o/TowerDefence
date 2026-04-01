using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 2f; // Скорость студента

    private Waypoints targetPath; // Путь, по которому идем
    private Transform targetPoint; // Конкретная точка, к которой стремимся сейчас
    private int pointIndex = 0;   // Номер текущей точки в массиве

    // Этот метод мы вызываем из спавнера, чтобы дать врагу маршрут
    public void SetupPath(Waypoints path)
    {
        targetPath = path;
        pointIndex = 0;

        if (targetPath.points.Length > 0)
        {
            targetPoint = targetPath.points[0];
        }
    }

    void Update()
    {
        // Если пути нет или мы дошли до конца - ничего не делаем
        if (targetPoint == null) return;

        // 1. ВЫЧИСЛЯЕМ НАПРАВЛЕНИЕ
        Vector3 direction = targetPoint.position - transform.position;

        // 2. ДВИГАЕМ ОБЪЕКТ
        // normalized делает вектор равным 1, чтобы скорость была одинаковой во все стороны
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // 3. ПОВОРОТ (необязательно для спрайтов, но полезно)
        // Если хочешь, чтобы спрайт просто двигался без вращения, можно закомментировать это
        if (direction != Vector3.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // 4. ПРОВЕРКА: ДОШЛИ ЛИ ДО ТОЧКИ?
        // Если расстояние до точки меньше 0.1 - переходим к следующей
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        // Проверяем, не была ли это последняя точка в массиве
        if (pointIndex >= targetPath.points.Length - 1)
        {
            ReachEnd();
            return;
        }

        // Переходим к следующей точке
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    void ReachEnd()
    {
        // Сюда мы потом впишем урон 14-му корпусу!
        Debug.Log("Враг прорвался в 14 корпус! Минус жизни!");
        Destroy(gameObject); // Враг исчезает, так как он "зашел" в здание
    }
}