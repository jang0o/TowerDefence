using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Placement Settings")]
    public GameObject towerPrefab;
    public Color hoverColor = Color.cyan;
    public Color startColor = Color.gray;
    
    private GameObject currentTower;
    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
        
        SetColor(startColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentTower == null && rend != null)
        {
            SetColor(hoverColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rend != null)
        {
            SetColor(startColor);
        }
    }

    private void SetColor(Color color)
    {
        if (rend == null) return;
        if (propBlock == null) propBlock = new MaterialPropertyBlock();
        
        rend.GetPropertyBlock(propBlock);
        propBlock.SetColor("_Color", color);
        rend.SetPropertyBlock(propBlock);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentTower != null)
        {
            Debug.Log("Plot already occupied!");
            return;
        }

        BuildTower();
    }

    void BuildTower()
    {
        if (towerPrefab == null)
        {
            Debug.LogWarning("No Tower Prefab assigned to " + gameObject.name);
            return;
        }

        TowerAI ai = towerPrefab.GetComponent<TowerAI>();
        int cost = (ai != null && ai.towerStats != null) ? ai.towerStats.cost : 100;

        if (CurrencyManager.TryPurchase(cost))
        {
            currentTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
            
            Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
            foreach(var r in allRenderers) r.enabled = false;
                
            Debug.Log("Tower purchased and placed on " + transform.parent.name);
        }
        else
        {
            Debug.Log("Not enough money to buy " + towerPrefab.name);
        }
    }
}
