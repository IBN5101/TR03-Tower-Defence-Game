using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform enemyPrefab = Resources.Load<Transform>("pf_Enemy");
        Transform enemyTransform = Instantiate(enemyPrefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();

        return enemy;
    }

    private Rigidbody2D rigidbody2d;
    private Transform targetTransform;
    private HealthSystem healthSystem;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.5f;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        // Randomized so that batches of enemy spawned on the same frame
        // don't look for targets at the same time
        lookForTargetTimer = Random.Range(0f, lookForTargetTimerMax);
    }

    private void Start()
    {
        targetTransform = BuildingManager.Instance.GetHQBuilding().transform;

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += Enemy_OnDied;
    }

    private void Enemy_OnDied(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();
    }

    private void HandleMovement()
    {
        if (targetTransform)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;
            float moveSpeed = 5f;

            rigidbody2d.velocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building)
        {
            // => Collided with a building!
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            Destroy(gameObject);
        }
    }

    private void LookForTargets()
    {
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider2D in collider2DArray)
        {
            Building building = collider2D.GetComponent<Building>();
            if (building)
            {
                // => Is a building!
                if (targetTransform == null)
                {
                    // => No target yet
                    targetTransform = building.transform;
                }
                else
                {
                    // => Compare distance
                    float newDistance = Vector3.Distance(transform.position, building.transform.position);
                    float currentDistance = Vector3.Distance(transform.position, targetTransform.position);
                    if (newDistance < currentDistance)
                    {
                        // => Switch to closer target
                        targetTransform = building.transform;
                    }
                }
            }
        }

        if (targetTransform == null)
        {
            // => No target within range
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
    }
}
