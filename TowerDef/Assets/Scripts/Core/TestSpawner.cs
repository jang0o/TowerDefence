using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [Header("��������� �����")]
    public BaseEnemy enemyToTest;

    public Waypoints pathToFollow;

    void Start()
    {
        if (enemyToTest != null && pathToFollow != null)
        {
            enemyToTest.SetupPath(pathToFollow);
            Debug.Log("���� �������: ���� ����� �������� �� ���� " + pathToFollow.name);
        }
        else
        {
            Debug.LogError("������: ������� ������ ���� � ������� _TestManager!");
        }
    }
}
