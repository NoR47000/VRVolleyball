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

    bool BallInZone()
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
        float distanceToBall = Mathf.Sqrt(Mathf.Pow((ball.transform.position.x-transform.position.x),2)+Mathf.Pow((ball.transform.position.z - transform.position.z),2));
        if (BallInZone() && grabDistance>=distanceToBall && ballRB.velocity==null)
        {
            Vector3 movePlayer = Vector3.MoveTowards(transform.position, ball.transform.position,2.0f);
            transform.Translate(movePlayer * Time.deltaTime * moveSpeed);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("The ball is in the zone!");
        }
    }*/
}
