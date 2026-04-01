using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public EnemyMovement enemyToTest; // Перетащим сюда наш красный кубик
    public Waypoints pathToFollow;   // Перетащим сюда Path_Main

    void Start()
    {
        if (enemyToTest != null && pathToFollow != null)
        {
            // Говорим кубику: "Твой путь - вот этот"
            enemyToTest.SetupPath(pathToFollow);
        }
    }
}