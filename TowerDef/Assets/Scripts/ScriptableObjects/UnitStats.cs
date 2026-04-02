using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats/GenericStats")]
public class UnitStats : ScriptableObject
{
    [Header("Obschie nastroyki (Dlya vseh)")]
    public string objectName;      // Imya ob'ekta (naprimer, "Admin Korpus")
    public float health = 100f;    // Zdorov'e (esli vragu mozhno bit' bashnyu)

    [Header("Nastroyki Vraga")]
    public float speed = 3f;       // Skorost' vraga
    public int reward = 50;        // Dengi za vraga

    [Header("Nastroyki Bashni")]
    public float range = 15f;      // Radius ataki
    public float fireRate = 1f;    // Skorost' strel'by
    public float damage = 20f;     // Uron odnoy puli
}