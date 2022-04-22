using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

        int nearbyResourceAmount = 0;
        foreach (Collider2D collider2D in collider2DArray)
        {
            ResourceNode resourceNode = collider2D.GetComponent<ResourceNode>();
            if (resourceNode && resourceNode.resourceType == resourceGeneratorData.resourceType)
            {
                nearbyResourceAmount++;
            }
        }
        nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
        return nearbyResourceAmount;
    }

    private ResourceGeneratorData resourceGeneratorData;
    private float timer;
    private float timerMax;

    private void Awake()
    {
        resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
        timerMax = resourceGeneratorData.timerMax;
    }

    private void Start()
    {
        int nearbyResourceAmount = GetNearbyResourceAmount(resourceGeneratorData, transform.position);

        // Adjust generation
        if (nearbyResourceAmount == 0)
        {
            // If no resource nodes nearby
            // Disable generation
            timerMax = -1;
            enabled = false;
        }
        else
        {
            // If MAX efficiency, timer = 0.5x original
            // LOWER efficiency, timer increase toward timer = 1.5x original
            timerMax = resourceGeneratorData.timerMax * 
                (1.5f - ((float)nearbyResourceAmount / resourceGeneratorData.maxResourceAmount));
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer += timerMax;
            ResourceManager.Instance.AddResource(resourceGeneratorData.resourceType, 1);
        }
    }

    public ResourceGeneratorData GetResourceGeneratorData()
    {
        return resourceGeneratorData;
    }

    public float GetTimerNormalized()
    {
        // For disabled generators
        if (timerMax < 0)
            return 1;

        return timer / timerMax;
    }

    public float GetResourceGeneratedPerSecond()
    {
        // For disabled generators
        if (timerMax < 0)
            return 0;

        return 1 / timerMax;
    }
}
