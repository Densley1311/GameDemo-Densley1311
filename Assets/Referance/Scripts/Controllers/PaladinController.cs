using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinController : NPCController
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

    [Header("Summoning")]
    public GameObject knightPrefab;
    public float summonResetInterval = 20.0f;
    private float summonTimer = 0.0f;
    public Transform[] summonPlatforms;

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
                else
                {
                    animator.SetBool("isAttaking", false);
                }
            }
        }
        else
        {
            target = gameObject.transform;
            animator.SetBool("isAttaking", false);
        }
        if (!(gameObject.GetComponent<Health>().currentHealth <= 0))
        {
            if ((gameObject.GetComponent<Health>().maxHealth / gameObject.GetComponent<Health>().currentHealth) >= 2)
            {
                summonTimer += Time.deltaTime;
                if (summonTimer >= summonResetInterval)
                {
                    SummonKnights();
                    summonTimer = 0.0f;
                }
            }
        }
        base.FixedUpdate();
    }

    void SummonKnights()
    {
        foreach (Transform spawner in summonPlatforms)
        {
            GameObject newKnight = Instantiate(knightPrefab, spawner.transform.position, spawner.transform.rotation);
        }
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
