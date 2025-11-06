using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // De snelheid van de speler.
    public float moveSpeed = 1f;
    // Het lichaam van de spelers waardoor zwaartekracht kan bestaan.
    public Rigidbody2D rb;
    // De Camera van de speler
    [SerializeField]
    private Camera mainCamera;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        
        movement = movement.normalized;

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
