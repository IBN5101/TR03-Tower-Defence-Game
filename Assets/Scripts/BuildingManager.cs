using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private Camera mainCamera;
    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO buildingType;

    private void Awake()
    {
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        buildingType = buildingTypeList.list[0];
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(buildingType.prefab, getMouseWorldPosition(), Quaternion.identity);
        }

        // Changes building type
        if (Input.GetKeyDown(KeyCode.Alpha1))
            buildingType = buildingTypeList.list[0];
        if (Input.GetKeyDown(KeyCode.Alpha2))
            buildingType = buildingTypeList.list[1];
        if (Input.GetKeyDown(KeyCode.Alpha3))
            buildingType = buildingTypeList.list[2];
    }

    private Vector3 getMouseWorldPosition()
    {
        Vector3 mouseWorldPostion = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPostion.z = 0f;
        return mouseWorldPostion;
    }
}
