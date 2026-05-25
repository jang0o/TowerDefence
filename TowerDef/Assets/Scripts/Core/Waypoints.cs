using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public Transform[] points;

    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < points.Length - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
                Gizmos.DrawSphere(points[i].position, 0.3f);
            }
        }
        if (points[points.Length - 1] != null)
            Gizmos.DrawSphere(points[points.Length - 1].position, 0.3f);
    }
}
