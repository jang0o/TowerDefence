using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI buildingsStatusText;

    private List<BaseEntity> allBuildings = new List<BaseEntity>();

    void Start()
    {
        RefreshBuildingList();
    }

    public void RefreshBuildingList()
    {
        allBuildings.Clear();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Building");
        foreach (var go in gos)
        {
            BaseEntity entity = go.GetComponent<BaseEntity>();
            if (entity != null) allBuildings.Add(entity);
        }
    }

    void Update()
    {
        if (WaveManager.Instance != null)
        {
            if (waveText != null) waveText.text = "Wave: " + WaveManager.Instance.currentWave;
            
            float t = WaveManager.Instance.timer;
            if (t < 0) t = 0;
            int minutes = Mathf.FloorToInt(t / 60);
            int seconds = Mathf.FloorToInt(t % 60);
            if (timerText != null) timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        if (moneyText != null)
        {
            moneyText.text = "Money: " + CurrencyManager.Money;
        }

        if (buildingsStatusText != null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<b>BUILDINGS HP:</b>");
            foreach (var b in allBuildings)
            {
                if (b != null)
                {
                    string displayName = GetRussianName(b.gameObject.name);
                    sb.AppendLine($"{displayName}: <color=blue>{Mathf.CeilToInt(b.health)}</color>");
                    }
                    }
            buildingsStatusText.text = sb.ToString();
        }
    }

    private string GetRussianName(string originalName)
    {
        string lowerName = originalName.ToLower();
        switch (lowerName)
        {
            case "angar": return "Ангар";
            case "10k": return "10 Корпус";
            case "6k": return "6 Корпус";
            case "18k": return "18 Корпус";
            case "7_obsh": return "7 Общежитие";
            case "ahc": return "АХЧ";
            case "6_obsh": return "6 Общежитие";
            case "11k": return "11 Корпус";
            case "admin+3": return "Админка";
            case "14k+7k": return "14 Корпус";
            default: return originalName;
        }
    }
    }
