using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static int Money = 250;

    public static bool TryPurchase(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            Debug.Log("Transaction successful! Remaining money: " + Money);
            return true;
        }
        else
        {
            Debug.Log("Not enough money! Need: " + amount + ", Have: " + Money);
            return false;
        }
    }

    public static void AddMoney(int amount)
    {
        Money += amount;
    }

    public static void SubtractMoney(int amount)
    {
        Money -= amount;
        if (Money < 0) Money = 0;
    }
}
