using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
    [Header("Настройки поезда")]
    public float speed = 2f;
    public float health = 500f;

    [Header("Настройки десанта")]
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnDelay = 0.5f;
    public Waypoints enemyPath;     // <-- СЮДА ТЯНИ НОВЫЙ ПУТЬ ДЛЯ ВРАГОВ

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;

    public void SetupTrain(Waypoints path, GameObject prefab)
    {
        targetPath = path;
        enemyPrefab = prefab;
        if (targetPath != null && targetPath.points.Length > 0)
            targetPoint = targetPath.points[0];
    }

    void Update()
    {
        if (targetPoint == null) return;

        Vector3 direction = targetPoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        if (targetPath == null || pointIndex >= targetPath.points.Length - 1)
        {
            StartCoroutine(UnloadEnemies());
            targetPoint = null;
            return;
        }

        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    IEnumerator UnloadEnemies()
    {
        Debug.Log("Поезд прибыл! Высадка десанта на новый путь...");

        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyPrefab != null && enemyPath != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Передаем врагу именно НОВЫЙ путь, который ты укажешь в инспекторе
                BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.SetupPath(enemyPath);
                }
            }
            else if (enemyPath == null)
            {
                Debug.LogError("ВНИМАНИЕ: Ты забыл перетащить Enemy Path в настройках поезда!");
            }

            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(gameObject);
    }
}