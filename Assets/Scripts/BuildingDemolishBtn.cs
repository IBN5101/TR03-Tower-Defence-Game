using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{
    [SerializeField] Building building;
    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => {
            BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;
            foreach (ResourceAmount resourceAmount in buildingType.constructionResourceCostArray)
            {
                int amountRefund = Mathf.FloorToInt(resourceAmount.amount * .6f);
                ResourceManager.Instance.AddResource(resourceAmount.resourceType, amountRefund);
            }
            Destroy(building.gameObject);
        });
    }
}
