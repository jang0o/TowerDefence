using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [Header("Настройки теста")]
    // Теперь здесь тип BaseEnemy, который мы создали для 3D
    public BaseEnemy enemyToTest;

    // Ссылка на точки пути (Path_Main)
    public Waypoints pathToFollow;

    void Start()
    {
        // Проверяем, всё ли мы перетащили в инспекторе
        if (enemyToTest != null && pathToFollow != null)
        {
            // Передаем врагу маршрут
            enemyToTest.SetupPath(pathToFollow);
            Debug.Log("Тест запущен: Враг начал движение по пути " + pathToFollow.name);
        }
        else
        {
            Debug.LogError("Ошибка: Заполни пустые поля в объекте _TestManager!");
        }
    }
}