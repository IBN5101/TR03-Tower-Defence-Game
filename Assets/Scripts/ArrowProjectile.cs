using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform arrowPrefab = Resources.Load<Transform>("pf_ArrowProjectile");
        Transform arrowTransform = Instantiate(arrowPrefab, position, Quaternion.identity);

        ArrowProjectile arrow = arrowTransform.GetComponent<ArrowProjectile>();
        arrow.SetTarget(enemy);

        return arrow;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;
    private float timeToDie = 2f;

    private void Update()
    {
        Vector3 moveDir;
        if (targetEnemy)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }
        float moveSpeed = 20f;

        transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        timeToDie -= Time.deltaTime;
        if (timeToDie < 0f)
            Destroy(gameObject);
    }

    public void SetTarget(Enemy targetEnemy)
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            // => Hit an enemy!
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);
            Destroy(gameObject);
        }
    }
}
