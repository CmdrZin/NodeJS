using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * This performs the same actions as PlayerController, but for network inputs of Force and Speed.
 */

public class RemotePlayer : MonoBehaviour
{
    public float speed = 0.0f;
    public string id = "Stan";
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

    // Called by Network to update the Remote Player's force changes.
    public void SetForce(Vector2 forceValue)
    {
        forceX = forceValue.x;
        forceY = forceValue.y;
    }
}
