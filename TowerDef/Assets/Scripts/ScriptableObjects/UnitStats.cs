using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats/GenericStats")]
public class UnitStats : ScriptableObject
{
    [Header("Obschie nastroyki (Dlya vseh)")]
    public string objectName;
    public float health = 100f;

    [Header("Nastroyki Vraga")]
    public float speed = 3f;
    public int reward = 50;

    [Header("Nastroyki Bashni")]
    public float range = 15f;
    public float fireRate = 1f;
    public float damage = 20f;
    public int cost = 100;
    }
