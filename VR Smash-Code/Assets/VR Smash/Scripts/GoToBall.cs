using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class GoToBall : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    // Ball RigidBody
    private Rigidbody ballRB;

    // The player's zone
    public Transform zone;

    private void Awake()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    // Get GrabThrowBall script for grabDistance
    public GrabThrowBall grabThrowBall;

    private float grabDistance;

    // Player's Move speed
    private float moveSpeed=10.0f;

    private void Start()
    {
        grabThrowBall = GetComponent<GrabThrowBall>();
        grabDistance = grabThrowBall.grabDistance;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    public bool BallInZone()
    {
        bool ballInZone = false;
        if (ballRB.position.x >= zone.position.x - zone.localScale.x / 2 &&
        ballRB.position.x <= zone.position.x + zone.localScale.x / 2 &&
        ballRB.position.z >= zone.position.z - zone.localScale.z / 2 &&
        ballRB.position.z <= zone.position.z + zone.localScale.z / 2)
        {
            ballInZone = !ballInZone;
        }

        return ballInZone;
    }

    // Move player to the ball
    private void MovePlayer()
    {
        // Get the ball position on the x,z plain
        Vector3 ballPositionProjected = new Vector3(ball.transform.position.x,0, ball.transform.position.z);

        // Get player position in x,z plain
        Vector3 positionProjected = new Vector3(transform.position.x, 0, transform.position.z);

        // Get distance to ball
        float distanceToBall = Vector3.Distance(ballPositionProjected, positionProjected);
        
        if (BallInZone() && (grabDistance-0.1)<=distanceToBall && ballRB.velocity==Vector3.zero)
        {
            // Calculate the new position the player will have
            Vector3 movePlayer = Vector3.MoveTowards(transform.position, ball.transform.position, Time.deltaTime * moveSpeed);
            movePlayer = new Vector3(movePlayer.x, transform.position.y, movePlayer.z);
            transform.position = movePlayer;
        }
    }
}
