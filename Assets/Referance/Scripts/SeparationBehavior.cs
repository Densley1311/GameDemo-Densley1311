using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SeparationBehavior : MonoBehaviour
{
    public float separationDistance = 1.0f; // Adjust this distance based on your game's needs
    public float maxSpeed = 5.0f; // Maximum movement speed of the entity
    public float separationForce = 50.0f; // Adjust the force to control the separation behavior

    // Reference to the list of nearby entities (e.g., other zombies)
    public GameObject[] nearbyEntities;
    private string alliesTag = "Ally";


    public float resetInterval = 1.0f; // Adjust this to set how often you want to reset velocities

    private float timer = 0.0f;

    private Rigidbody2D rb; // Assuming you're using Rigidbody2D for movement

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        nearbyEntities = GameObject.FindGameObjectsWithTag(alliesTag);

        Vector2 separationDirection = Vector2.zero;

        foreach (GameObject entity in nearbyEntities)
        {
            if (entity != this.gameObject) // Don't consider self
            {
                Vector2 offset = entity.transform.position - transform.position;
                float distance = offset.magnitude;

                if (distance < separationDistance)
                {
                    // Calculate a force to move away from the nearby entity
                    Vector2 separationForceVector = -offset.normalized * (1.0f - distance / separationDistance) * separationForce;

                    separationDirection += separationForceVector;
                }
            }
        }

        // Apply the separation force to the entity's movement
        Vector2 finalVelocity = rb.linearVelocity + separationDirection * Time.deltaTime;
        finalVelocity = Vector2.ClampMagnitude(finalVelocity, maxSpeed);
        rb.linearVelocity = finalVelocity;

        timer += Time.deltaTime;

        if (timer >= resetInterval)
        {
            ResetVelocities();
            timer = 0.0f; // Reset the timer
        }
    }

    private void ResetVelocities()
    {
        
            rb.linearVelocity = Vector2.zero;
        
    }
}