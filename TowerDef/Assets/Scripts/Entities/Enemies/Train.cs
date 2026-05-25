using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
    [Header("Train Settings")]
    public float speed = 5f;
    public float health = 500f;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public int enemyCount = 3;
    public float spawnDelay = 0.5f;
    public Waypoints enemyPath;

    public int stopWaypointIndex = 3;

    private Waypoints targetPath;
    private Transform targetPoint;
    private int pointIndex = 0;
    private bool isUnloading = false;

    public void SetupTrain(Waypoints path, GameObject prefab)
    {
        targetPath = path;
        enemyPrefab = prefab;
        pointIndex = 0;
        if (targetPath != null && targetPath.points.Length > 0)
        {
            targetPoint = targetPath.points[0];
            transform.position = targetPoint.position;
        }
    }

    void Update()
    {
        if (targetPoint == null || isUnloading) return;

        Vector3 direction = targetPoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        {
            GetNextPoint();
        }
    }

    void GetNextPoint()
    {
        if (targetPath == null || pointIndex >= targetPath.points.Length - 1 || pointIndex >= stopWaypointIndex)
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
        isUnloading = true;
        Debug.Log("Train arrived. Unloading enemies...");

        for (int i = 0; i < enemyCount; i++)
        {
            if (enemyPrefab != null && enemyPath != null)
            {
                GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                BaseEnemy enemyScript = enemy.GetComponent<BaseEnemy>();
                if (enemyScript != null)
                {
                    enemyScript.SetupPath(enemyPath);
                }
            }
            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log("Unloading complete. Destroying train car.");
        Destroy(gameObject);
    }
}
