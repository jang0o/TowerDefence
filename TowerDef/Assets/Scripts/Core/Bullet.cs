using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target; // Cel' puly
    public float speed = 20f; // Skorost' poleta
    public float bulletDamage = 10f; // Uron, kotoryy peredast bashnya

    // Metod, chtoby bashnya mogla peredat' cel' pule
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // Esli cel' ischezla (naprimer, umerla ot drugoy bashni)
        if (target == null)
        {
            Destroy(gameObject); // Unichtozhaem pulyu
            return;
        }

        // Napravlenie k celi
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Proverka: doleteli li my v etom kadre
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Dvizhenie v mirovom prostranstve
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        // Ishem script vraga na ob'ekte celi
        BaseEnemy enemyScript = target.GetComponent<BaseEnemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(bulletDamage); // Nanosim uron
        }

        Destroy(gameObject); // Pulyu udalyaem posle popadaniya
    }
}