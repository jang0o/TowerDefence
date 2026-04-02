using UnityEngine;
using System.Collections;

public class TrainTransport : MonoBehaviour
{
    public UnitStats trainStats;    // Скорость самого поезда
    public GameObject enemyPrefab;  // Кто выйдет из поезда
    public int enemyCount = 5;      // Сколько врагов выйдет
    public float spawnDelay = 0.3f; // Скорость высадки

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;

    public void SetupTrain(Waypoints path, GameObject prefab)
    {
        targetPath = path;
        enemyPrefab = prefab;
        if (targetPath.points.Length > 0)
            targetPoint = targetPath.points[0];
    }

    void Update()
    {
        if (targetPoint == null) return;

        // Движение поезда
        Vector3 direction = targetPoint.position - transform.position;
        transform.Translate(direction.normalized * trainStats.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        if (pointIndex >= targetPath.points.Length - 1)
        {
            StartCoroutine(UnloadEnemies()); // Приехали! Высадка.
            targetPoint = null;
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    IEnumerator UnloadEnemies()
    {
        Debug.Log("Поезд прибыл! Высадка врагов...");
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().SetupPath(targetPath); // Враги тоже идут по пути
            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(gameObject); // Поезд исчезает после высадки
    }
}