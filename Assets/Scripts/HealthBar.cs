using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    private Transform barTransform;
    private Transform separatorContainer;

    private void Awake()
    {
        barTransform = transform.Find("bar");
        separatorContainer = transform.Find("separatorContainer");
    }

    private void Start()
    {
        ConstructHealthBarSeparators();

        UpdateBar();
        UpdateHealthBarVisible();

        healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, System.EventArgs e)
    {
        ConstructHealthBarSeparators();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        // Hide if full health
        if (healthSystem.IsFullHealth())
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    private void ConstructHealthBarSeparators()
    {
        // Health bar separators
        Transform separatorTemplate = separatorContainer.Find("separatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        // Destroy previously created separators
        foreach (Transform separatorTransform in separatorContainer)
        {
            if (separatorTransform == separatorTemplate)
                continue;
            Destroy(separatorTransform.gameObject);
        }

        int healthAmountPerSeparator = 10;
        float barSize = 3f;
        float barOneSeparatorSize = barSize / healthSystem.GetHealthAmountMax();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);
        for (int i = 1; i < healthSeparatorCount; i++)
        {
            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneSeparatorSize * i * healthAmountPerSeparator, 0, 0);
        }
    }
}
