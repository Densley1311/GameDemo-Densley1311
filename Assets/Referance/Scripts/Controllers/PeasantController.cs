using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantController : NPCController
{
    [Header("Attack")]
    public float findResetInterval = 5.0f;
    private float findTimer = 0.0f;

    [SerializeField]
    float enemyDistance;
    [SerializeField]
    float attackDistance;

    [SerializeField]
    int attackDamage;
    public float attackResetInterval = 2.0f;
    private float attackTimer = 0.0f;
    public override void FixedUpdate()
    {
        findTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (findTimer >= findResetInterval)
        {
            FindClosestEnemy();
            findTimer = 0.0f;
        }

        if (opponent != null)
        {
            if (Vector2.Distance(opponent.transform.position, gameObject.transform.position) < enemyDistance)
            {
                target = opponent.transform;
                minimumRange = attackDistance;

                if (Vector2.Distance(opponent.transform.position, gameObject.transform.position) < attackDistance)
                {
                    Attack(opponent);
                }
            }
        }
        else
        {
            target = gameObject.transform;
        }

        base.FixedUpdate();
    }

    void Attack(GameObject attackTarget)
    {
        if (attackTimer >= attackResetInterval)
        {
            attackTarget.GetComponent<Health>().TakeDamage(attackDamage);
            attackTimer = 0.0f;
        }
    }
}
