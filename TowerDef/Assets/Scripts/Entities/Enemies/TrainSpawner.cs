using UnityEngine;

public class TrainSpawner : MonoBehaviour
{
    public GameObject trainPrefab;
    public GameObject enemyInTrainPrefab;
    public Waypoints pathToFollow;
    public float spawnRate = 20f;

    void Start()
    {
        InvokeRepeating("SpawnTrain", 2f, spawnRate);
    }

    void SpawnTrain()
    {
        if (trainPrefab == null)
        {
            Debug.LogError("Vnimanie! Ty zabyl polozhit' prefab poezda v TrainSpawner!");
            return;
        }

        GameObject train = Instantiate(trainPrefab, transform.position, Quaternion.identity);

        Train trainScript = train.GetComponent<Train>();

        if (trainScript != null)
        {
            trainScript.SetupTrain(pathToFollow, enemyInTrainPrefab);
        }
    }
}
