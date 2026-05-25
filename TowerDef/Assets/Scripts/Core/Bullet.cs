using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 20f;
    public float bulletDamage = 10f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        BaseEnemy enemyScript = target.GetComponent<BaseEnemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}
