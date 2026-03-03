using UnityEngine;
using System.Collections.Generic;

public class PathSystem : MonoBehaviour
{
    [Header("Настройки пути")]
    public List<Transform> waypoints = new List<Transform>();
    public Color pathColor = Color.red;
    public float waypointSize = 0.3f;

    void Awake()
    {
        // Автоматически собираем все дочерние точки
        waypoints.Clear();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        Debug.Log("Найдено точек пути: " + waypoints.Count);
    }

    // Визуализация пути в редакторе
    void OnDrawGizmos()
    {
        // Очищаем список и собираем заново
        waypoints.Clear();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        if (waypoints.Count < 2) return;

        Gizmos.color = pathColor;

        // Рисуем линии между точками
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                Gizmos.DrawSphere(waypoints[i].position, waypointSize);
            }
        }

        // Рисуем последнюю точку
        if (waypoints.Count > 0 && waypoints[waypoints.Count - 1] != null)
        {
            Gizmos.color = Color.blue; // Финиш синим
            Gizmos.DrawSphere(waypoints[waypoints.Count - 1].position, waypointSize);

            Gizmos.color = Color.green; // Старт зеленым
            Gizmos.DrawSphere(waypoints[0].position, waypointSize);
        }
    }
}