using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : NPCController
{
    [Header("Waypoint")]
    public Transform waypoint;
    [SerializeField]
    float waypointDistance;
    [SerializeField]
    float waypointAttackDistance;

    [Header("Attack")]
    public float findResetInterval = 5.0f;
    private float findTimer = 0.0f;

    [SerializeField]
    float enemyDistance;
    [SerializeField]
    float attackDistance;

    public int attackDamage;
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

        if (Vector2.Distance(waypoint.position, gameObject.transform.position) > waypointDistance)
        {
            target = waypoint;
            minimumRange = waypointDistance;
        }

        if (opponent != null)
        {
            if ((Vector2.Distance(waypoint.position, gameObject.transform.position) <= waypointAttackDistance) &&
                Vector2.Distance(opponent.transform.position, gameObject.transform.position) < enemyDistance)
            {
                target = opponent.transform;
                minimumRange = attackDistance;
                if (Vector2.Distance(opponent.transform.position, gameObject.transform.position) < attackDistance)
                {
                    Attack(opponent);
                } else
                {
                    animator.SetBool("isAttaking", false);
                }
            }
        }
        else {
            target = waypoint;
            animator.SetBool("isAttaking", false);
        }

        base.FixedUpdate();
    }

    void Attack(GameObject attackTarget)
    {
        if (attackTimer >= attackResetInterval)
        {
            animator.SetBool("isAttaking", true);
            attackTarget.GetComponent<Health>().TakeDamage(attackDamage);
            attackTimer = 0.0f;
        }
    }
}
