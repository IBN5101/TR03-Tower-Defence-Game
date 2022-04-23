using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] HealthSystem healthSystem;

    private Transform barTransform;

    private void Awake()
    {
        barTransform = transform.Find("bar");
    }

    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
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
}
