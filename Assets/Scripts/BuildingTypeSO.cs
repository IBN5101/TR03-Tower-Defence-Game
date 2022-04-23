using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BuildingType")]
public class BuildingTypeSO : ScriptableObject
{
    public string nameString;
    public Transform prefab;
    public ResourceGeneratorData resourceGeneratorData;
    public Sprite sprite;
    public float minConstructionRadius;
    public ResourceAmount[] constructionResourceCostArray;
    public int healthAmountMax;

    public string GetConstructionCostArrayString()
    {
        string str = "";
        foreach (ResourceAmount resourceAmount in constructionResourceCostArray)
        {
            str += "<color=#" + resourceAmount.resourceType.colorHex + ">";
            str += resourceAmount.resourceType.nameShort;
            str += resourceAmount.amount;
            str += "</color>";
            str += " ";
        }

        return str;
    }
}
