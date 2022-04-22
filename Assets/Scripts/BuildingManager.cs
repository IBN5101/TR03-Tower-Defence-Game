using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    // Singleton pattern
    public static BuildingManager Instance { get; private set; }

    // Events
    public event EventHandler<OnActiveBuildingTypeChangedEventArgs> OnActiveBuildingTypeChanged;
    public class OnActiveBuildingTypeChangedEventArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType && CanSpawnBuilding(activeBuildingType, UtilsClass.getMouseWorldPosition()))
            {
                if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                {
                    ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                    Instantiate(activeBuildingType.prefab, UtilsClass.getMouseWorldPosition(), Quaternion.identity);
                }
            }
        }

        // Changes building type
        // Temporary solution
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetActiveBuildingType(buildingTypeList.list[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetActiveBuildingType(buildingTypeList.list[1]);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetActiveBuildingType(buildingTypeList.list[2]);
    }

    public BuildingTypeSO GetActiveBuildingType()
    {
        return activeBuildingType;
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;

        OnActiveBuildingTypeChanged?.Invoke(this,
            new OnActiveBuildingTypeChangedEventArgs { activeBuildingType = activeBuildingType }
        );
    }

    public bool CanSpawnBuilding(BuildingTypeSO buildingType, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingType.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray;
        collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);
        bool isAreaClear = (collider2DArray.Length == 0);
        if (!isAreaClear)
            return false;

        // Buildings need to be a min distance from each other
        collider2DArray = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, buildingType.minConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            // Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder && buildingTypeHolder.buildingType == buildingType)
                return false;
        }

        // Building can not be spawned if it is too far from constructed buildings
        float maxConstructionRadius = 25;
        collider2DArray = Physics2D.OverlapCircleAll(position + (Vector3)boxCollider2D.offset, maxConstructionRadius);
        foreach (Collider2D collider2D in collider2DArray)
        {
            // Colliders inside the construction radius
            BuildingTypeHolder buildingTypeHolder = collider2D.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder)
                return true;
        }

        return false;
    }
}
