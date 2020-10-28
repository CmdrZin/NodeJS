using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// For inputs
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.0f;
    public string id = "Bob";
    public int color = 0;

    private Rigidbody rb;
    private float forceX;
    private float forceY;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Physics is applied here before scene rendering. Physics go here.
    void FixedUpdate()
    {
        // Convert Vector2 to Vector3 object.
        Vector3 force = new Vector3(forceX, 0.0f, forceY);
        // Apply force to object.
        rb.AddForce(force * speed);  // Multipies all vector elements by speed.
    }

    // This is called after all of the scene movements are completed.
    private void LateUpdate()
    {
        // Send position data to update other Clients.
        
    }

    // Return the force vector from components.
    public Vector2 GetForce()
    {
        return (new Vector2(forceX, forceY));
    }

    // Called by the Player Input->Input Action Asset. Responds to WASD keys.
    void OnMove(InputValue movementValue)
    {
        Vector2 forceVector = movementValue.Get<Vector2>();

        forceX = forceVector.x;
        forceY = forceVector.y;

    }
}
