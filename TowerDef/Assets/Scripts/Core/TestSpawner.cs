using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    [Header("��������� �����")]
    // ������ ����� ��� BaseEnemy, ������� �� ������� ��� 3D
    public BaseEnemy enemyToTest;

    // ������ �� ����� ���� (Path_Main)
    public Waypoints pathToFollow;

    void Start()
    {
        // ���������, �� �� �� ���������� � ����������
        if (enemyToTest != null && pathToFollow != null)
        {
            // �������� ����� �������
            enemyToTest.SetupPath(pathToFollow);
            Debug.Log("���� �������: ���� ����� �������� �� ���� " + pathToFollow.name);
        }
        else
        {
            Debug.LogError("������: ������� ������ ���� � ������� _TestManager!");
        }
    }
}