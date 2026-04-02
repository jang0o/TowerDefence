using UnityEngine; // ВОТ ЭТОЙ СТРОЧКИ У ТЕБЯ НЕ ХВАТАЛО!

public class TrainSpawner : MonoBehaviour
{
    public GameObject trainPrefab; // Префаб вагона
    public GameObject enemyInTrainPrefab; // Префаб врага-кубика
    public Waypoints pathToFollow;
    public float spawnRate = 20f;

    void Start()
    {
        // Запускаем спавн поезда через 2 секунды после старта, повторяем каждые spawnRate секунд
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

        // ВАЖНО: Если ты переименовал скрипт поезда в "Train", 
        // то замени TrainTransport на Train в строчке ниже:
        Train trainScript = train.GetComponent<Train>();

        if (trainScript != null)
        {
            trainScript.SetupTrain(pathToFollow, enemyInTrainPrefab);
        }
    }
}