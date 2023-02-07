using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBall : MonoBehaviour
{
    public Transform player;
    public Rigidbody ball;

    void Update()
    {
        // Tests if button Fire1 is pressed on controller (configure in Unity parameters)
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 force = player.forward * 1000f;
            ball.AddForce(force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Simulates interaction with external world
        if (collision.gameObject.name == "Ball")
        {
            Vector3 velocity = ball.velocity;
            velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
            ball.velocity = velocity;
        }
    }
}
