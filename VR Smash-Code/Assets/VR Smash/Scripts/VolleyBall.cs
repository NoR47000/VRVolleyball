using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBall : MonoBehaviour
{
    // Movement value under which the ball is stopped
    public float stopThreshold = 0.1f;

    // Height under which the code enters the if
    private float groundThreshold = 0.5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Check if ball is on the ground
        bool ground = (transform.position.y < groundThreshold);

        // Stop the ball's movement if it's on the ground and it is almost stopped
        // Prevents the ball from rotating indefinitily
        if (ground && Quaternion.Angle(transform.rotation, Quaternion.identity) < stopThreshold && rb.velocity.magnitude < stopThreshold && rb.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    //private void OnCollisionEnter(Collider other)
    //{
    //    if(other.gameObject.CompareTag("EnemySide")|| other.gameObject.CompareTag("AlliedSide"))
    //    {
    //        rb.velocity = Vector3.zero;
    //        rb.angularVelocity = Vector3.zero;
    //    }
    //}
}
