using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("Staty korpusa")]
    public UnitStats towerStats;    // Ssylka na nash novyy ScriptableObject

    [Header("Tehnicheskie ssylki")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private Transform target;
    private float fireCountdown = 0f;

    void Update()
    {
        // Teper' berem radius iz towerStats
        FindTarget();

        if (target == null) return;

        if (fireCountdown <= 0f)
        {
            Shoot();
            // Berem skorost' strel'by iz towerStats
            fireCountdown = 1f / towerStats.fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            // Proveryaem radius iz statov
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= towerStats.range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
            target = nearestEnemy.transform;
        else
            target = null;
    }

    void Shoot()
    {
        GameObject projGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Bullet bullet = projGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            // Peredaem uron iz statov bashni v pulyu
            bullet.Seek(target);
            bullet.bulletDamage = towerStats.damage;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (towerStats == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, towerStats.range);
    }
}