using UnityEngine;
using System.Collections;

public class TrainTransport : MonoBehaviour
{
    public UnitStats trainStats;
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnDelay = 0.3f;

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
            StartCoroutine(UnloadEnemies());
            targetPoint = null;
            return;
        }
        pointIndex++;
        targetPoint = targetPath.points[pointIndex];
    }

    IEnumerator UnloadEnemies()
    {
        Debug.Log("����� ������! ������� ������...");
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.GetComponent<BaseEnemy>().SetupPath(targetPath);
            yield return new WaitForSeconds(spawnDelay);
        }

        Destroy(gameObject);
    }
}
