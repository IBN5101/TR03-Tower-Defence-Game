using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Vector3 projectileSpawnPosition;

    private Enemy targetEnemy;

    private float lookForTargetTimer;
    [SerializeField] private float lookForTargetTimerMax = 0.5f;
    private float shootTimer;
    [SerializeField] private float shootTimerMax = 1f;
    [SerializeField] private float targetMaxRadius = 20f;

    private void Awake()
    {
        projectileSpawnPosition = transform.Find("projectileSpawnPosition").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();
    }

    private void HandleTargeting()
    {
        // Timer
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void HandleShooting()
    {
        // Timer
        shootTimer -= Time.deltaTime;
        if (shootTimer < 0f)
        {
            shootTimer += shootTimerMax;
            if (targetEnemy)
                ArrowProjectile.Create(projectileSpawnPosition, targetEnemy);
        }
    }

    private void LookForTargets()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy)
            {
                // => Is an enemy!
                if (targetEnemy == null)
                {
                    // => No target yet
                    targetEnemy = enemy;
                }
                else
                {
                    // => Compare distance
                    float newDistance = Vector3.Distance(transform.position, enemy.transform.position);
                    float currentDistance = Vector3.Distance(transform.position, targetEnemy.transform.position);
                    if (newDistance < currentDistance)
                    {
                        // => Switch to closer target
                        targetEnemy = enemy;
                    }
                }
            }
        }

        if (targetEnemy == null)
        {
            // => No target within range
            
        }
    }
}
