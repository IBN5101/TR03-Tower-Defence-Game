using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] List<BuildingTypeSO> ignoreBuildingTypeList;

    [SerializeField] private Sprite arrowSprite;
    private Transform arrowBtn;

    private BuildingTypeListSO buildingTypeList;
    private Dictionary<BuildingTypeSO, Transform> buildingTypeTransformDictionary;

    private Transform btnTemplate;

    private void Awake()
    {
        // Get building list & Init
        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
        buildingTypeTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
        // Get template from scene
        btnTemplate = transform.Find("btnTemplate");
        btnTemplate.gameObject.SetActive(false);

        // Arrow button
        CreateArrowButton();
        // Duplicate template and set correct properties
        int index = 1;
        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            // Check ignore list
            if (ignoreBuildingTypeList.Contains(buildingType))
                continue;

            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);
            // Position
            float offsetX = 130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetX * index, 0);
            // Image
            btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;
            // Button
            btnTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });
            // Button on-hover events
            MouseEnterExitEvent mouseEnterExitEvent = btnTransform.GetComponent<MouseEnterExitEvent>();
            mouseEnterExitEvent.OnMouseEnter += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Show(buildingType.nameString + "\n" + buildingType.GetConstructionCostArrayString());
            };
            mouseEnterExitEvent.OnMouseExit += (object sender, EventArgs e) =>
            {
                TooltipUI.Instance.Hide();
            };

            // Store reference to dictionary
            buildingTypeTransformDictionary[buildingType] = btnTransform;

            index++;
        }
    }

    private void Start()
    {
        UpdateActiveBuildingTypeButton();
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void CreateArrowButton()
    {
        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);
        // Position
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        // Image
        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -25);
        // Button
        arrowBtn.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
            UpdateActiveBuildingTypeButton();
        });
        // Button on-hover events
        MouseEnterExitEvent mouseEnterExitEvent = arrowBtn.GetComponent<MouseEnterExitEvent>();
        mouseEnterExitEvent.OnMouseEnter += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Show("Arrow");
        };
        mouseEnterExitEvent.OnMouseExit += (object sender, EventArgs e) =>
        {
            TooltipUI.Instance.Hide();
        };
    }

    private void UpdateActiveBuildingTypeButton()
    {
        arrowBtn.Find("selected").gameObject.SetActive(false);
        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            Transform btnTransform = buildingTypeTransformDictionary[buildingType];
            // Hide selected
            btnTransform.Find("selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType)
            buildingTypeTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
        else
            arrowBtn.Find("selected").gameObject.SetActive(true);
    }
}
