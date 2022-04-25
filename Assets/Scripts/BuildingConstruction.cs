using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Transform constructionPrefab = GameAssets.Instance.pf_BuildingConstruction;
        Transform constructionTransform = Instantiate(constructionPrefab, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = constructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }

    private float constructionTimer;
    private float constructionTimerMax;
    private BuildingTypeSO buildingType;

    private BoxCollider2D boxCollider2d;
    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;

        Instantiate(GameAssets.Instance.pf_BuildingPlacedParticles, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        constructionTimer -= Time.deltaTime;
        if (constructionTimer < 0f)
        {
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(GameAssets.Instance.pf_BuildingPlacedParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }

        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());
    }

    private void SetBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        boxCollider2d.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2d.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;
        spriteRenderer.sprite = buildingType.sprite;
        buildingTypeHolder.buildingType = buildingType;
    }

    public float GetConstructionTimerNormalized()
    {
        return 1f - constructionTimer / constructionTimerMax;
    }
}
