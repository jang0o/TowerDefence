using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Settings")]
    public float waveDuration = 300f; // 5 minutes
    public int currentWave = 0;
    public float timer;

    [Header("Enemy Prefabs")]
    public GameObject politehPrefab;
    public GameObject sgeyPrefab;
    public GameObject zhdEnemyPrefab;
    public GameObject zhdTrainPrefab;

    [Header("Spawning")]
    public Waypoints mainPath;
    public Waypoints path2; // Added Path2 support
    public Waypoints trainPath;
    public Waypoints trainUnloadPath;

    [Header("Scaling")]
    public float hpMultiplierPerWave = 1.2f;
    public float damageMultiplierPerWave = 1.1f;

    private bool waveInProgress = false;
    private bool useAlternatePath = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (waveInProgress)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                EndWave();
            }
        }
    }

    public void StartNextWave()
    {
        currentWave++;
        timer = waveDuration;
        waveInProgress = true;
        
        StartCoroutine(SpawnRoutine());
        Debug.Log("Wave " + currentWave + " started!");
    }

    IEnumerator SpawnRoutine()
    {
        int politehCount = 10 + (currentWave * 5);
        float politehInterval = waveDuration / (politehCount + 1);

        float sgeyTiming = waveDuration / 4f;
        int sgeySpawned = 0;

        float zhdTiming = waveDuration / (3 + (currentWave - 1) * 2 + 1f);
        int zhdSpawned = 0;
        int zhdMaxCount = 3 + (currentWave - 1) * 2;

        float elapsed = 0f;
        int politehSpawned = 0;

        while (elapsed < waveDuration && waveInProgress)
        {
            if (politehSpawned < politehCount && elapsed >= politehSpawned * politehInterval)
            {
                // Alternate between mainPath and path2
                Waypoints selectedPath = (useAlternatePath && path2 != null) ? path2 : mainPath;
                SpawnEnemy(politehPrefab, selectedPath);
                
                useAlternatePath = !useAlternatePath;
                politehSpawned++;
            }

            if (sgeySpawned < 3 && elapsed >= (sgeySpawned + 1) * sgeyTiming)
            {
                SpawnEnemy(sgeyPrefab, mainPath);
                sgeySpawned++;
            }

            if (zhdSpawned < zhdMaxCount && elapsed >= (zhdSpawned + 1) * zhdTiming)
            {
                SpawnTrain();
                zhdSpawned++;
            }

            elapsed += 1.0f;
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void SpawnEnemy(GameObject prefab, Waypoints path)
    {
        if (prefab == null || path == null) return;
        
        GameObject go = Instantiate(prefab, path.points[0].position, Quaternion.identity);
        BaseEnemy enemy = go.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.SetupPath(path);
            
            if (enemy.stats != null)
            {
                UnitStats runtimeStats = ScriptableObject.Instantiate(enemy.stats);
                runtimeStats.health *= Mathf.Pow(hpMultiplierPerWave, currentWave - 1);
                runtimeStats.damage *= Mathf.Pow(damageMultiplierPerWave, currentWave - 1);
                enemy.stats = runtimeStats;
                enemy.health = runtimeStats.health;
                enemy.maxHealth = runtimeStats.health;
            }
        }
    }

    public void SpawnTrain()
    {
        if (zhdTrainPrefab == null || trainPath == null || trainPath.points.Length == 0) return;
        
        Vector3 spawnPos = trainPath.points[0].position;
        GameObject trainGo = Instantiate(zhdTrainPrefab, spawnPos, Quaternion.identity);
        
        Train t = trainGo.GetComponent<Train>();
        if (t != null)
        {
            t.SetupTrain(trainPath, zhdEnemyPrefab);
            t.enemyPath = trainUnloadPath;
            t.stopWaypointIndex = 3;
            t.enemyCount = 3;
        }
    }

    void EndWave()
    {
        waveInProgress = false;
        Debug.Log("Wave " + currentWave + " ended!");
        Invoke("StartNextWave", 10f);
    }
}