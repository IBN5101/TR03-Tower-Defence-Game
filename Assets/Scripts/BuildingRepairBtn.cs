using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairBtn : MonoBehaviour
{
    [SerializeField] Building building;
    [SerializeField] ResourceTypeSO goldResourceType;

    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 3;
            ResourceAmount[] resourceAmountCost = {
                new ResourceAmount { resourceType = goldResourceType, amount = repairCost }
            };
            if (ResourceManager.Instance.CanAfford(resourceAmountCost))
            {
                // => Can afford to repair
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            }
            else
            {
                TooltipUI.Instance.Show("Cannot afford repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
            }
            
        });
    }
}
