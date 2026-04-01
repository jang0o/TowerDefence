using UnityEngine;

[System.Serializable] // Это позволит видеть настройки в инспекторе
public class UnitStats
{
    public float health = 100f;
    public float damage = 10f;
    public float speed = 3f;
    public int reward = 50; // Деньги за убийство врага
}