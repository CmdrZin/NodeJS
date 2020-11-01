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

    // change back to private after debugging.
    public Vector3 lastPosition;
    public Vector3 pDiff = new Vector3(0.0f, 0.0f, 0.0f);

    public Network network;         // Link to Network functions.

    private int frames;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
    }

    // Physics is applied here before scene rendering. Physics go here.
    void FixedUpdate()
    {
        //Update velocity. Units per frame.
        pDiff = lastPosition - transform.position;
        lastPosition = transform.position;

        // Send an update every N frames.
        if (Time.frameCount % 2 == 0)
        {
            network.SendUpdatePosition(transform.position);
        }

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

    // Return the velocity vector. Units changed per frame.
    public Vector3 GetVelocity()
    {
        return (pDiff);
    }

    // Called by the Player Input->Input Action Asset. Responds to WASD keys.
    void OnMove(InputValue movementValue)
    {
        Vector2 forceVector = movementValue.Get<Vector2>();

        forceX = forceVector.x;
        forceY = forceVector.y;

        // Send update to Server. Velocity, speed, and position.
//        network.SendUpdateMotion(pDiff, speed, transform.position);
    }
}
