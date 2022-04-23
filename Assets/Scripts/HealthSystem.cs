using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // Events
    public event EventHandler OnDamaged;
    public event EventHandler OnDied;

    [SerializeField] private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (isDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool isDead()
    {
        return healthAmount == 0;
    }

    public bool IsFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    public void SetHealthAmountMax(int amount, bool updateHealthAmount)
    {
        healthAmountMax = amount;
        if (updateHealthAmount)
            healthAmount = healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
}
