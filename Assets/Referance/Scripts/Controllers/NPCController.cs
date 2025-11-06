using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    public float moveSpeed;

    public Rigidbody2D rb;
    public Animator animator;

    [SerializeField]
    public Transform target;
    [SerializeField]
    public float minimumRange;

    public GameObject[] nearbyOpponents;
    [SerializeField]
    private string opponentTag;
    [SerializeField]
    public GameObject opponent;


    public virtual void FixedUpdate()
    {
        Movement(target, minimumRange);
    }

    void Movement(Transform currentTarget, float inRange)
    {
        //If the NPC is not within the range give then the NPC will walk there.
        if (Vector2.Distance(currentTarget.position, gameObject.transform.position) > inRange)
        {
            animator.SetFloat("Horizontal", currentTarget.position.x - transform.position.x);
            animator.SetFloat("Vertical", currentTarget.position.y - transform.position.y);
            animator.SetFloat("Speed", (currentTarget.position - transform.position).sqrMagnitude);

            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    public void FindClosestEnemy()
    {
        nearbyOpponents = GameObject.FindGameObjectsWithTag(opponentTag);

        GameObject closestOpponent = null;
        float minDistance = float.MaxValue;
        foreach (GameObject entity in nearbyOpponents)
        {
            // Calculate the distance to the current enemy
            float distanceToOpponent = Vector3.Distance(transform.position, entity.transform.position);

            // Check if this enemy is closer than the current closest enemy
            if (distanceToOpponent < minDistance)
            {
                minDistance = distanceToOpponent;
                closestOpponent = entity;
            }
        }

        if (closestOpponent != null)
        {
            opponent = closestOpponent;
        }
    }
}
