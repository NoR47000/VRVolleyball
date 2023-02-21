using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabThrowBall : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    // The player's grab distance
    public float grabDistance = 1.0f;

    //get ball rigidbody
    private Rigidbody ballRB;

    // The force to apply to the ball when it's thrown
    public float throwForce = 10.0f;

    // The player's throwing point
    public Transform throwPoint;

    // Is the player holding the ball?
    private bool holdingBall = false;

    private void Awake()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingBall && ballRB.velocity.magnitude <= 0 && Vector3.Distance(ball.transform.position, transform.position) <= grabDistance)
        {
            // Set the ball's position and rotation to match the player's throwing point
            ball.transform.position = throwPoint.position;
            ball.transform.rotation = throwPoint.rotation;

            // Make the ball a child of the player object
            ball.transform.parent = transform;

            // Set holdingBall to true
            holdingBall = true;
        }

        // If the player is holding the ball and the user clicks the mouse button
        if (holdingBall)
        {
            StartCoroutine(ThrowBallCoroutine());
        }
    }

    // The time delay before the ball is thrown
    public float throwDelay = 5.0f;

    private IEnumerator ThrowBallCoroutine()
    {
        // Set holdingBall to false
        holdingBall = false;

        yield return new WaitForSeconds(throwDelay);

        // Release the ball
        ball.transform.parent = null;

        // Apply a force to the ball
        ball.GetComponent<Rigidbody>().AddForce(throwPoint.forward *ballRB.mass*throwForce, ForceMode.Impulse);

    }
}
