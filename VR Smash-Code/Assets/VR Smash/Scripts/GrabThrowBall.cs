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

    // Has the ball been Thrown ?
    private bool hasBeenThrown =false;

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

        // If the player is holding the ball
        if (holdingBall && !hasBeenThrown)
        {
            // Orientation of throwPoint
            ThrowPointOrientation();

            StartCoroutine(ThrowBallCoroutine());

            // Set hasBeenThrown to true
            hasBeenThrown = true;
        }
    }

    // The time delay before the ball is thrown
    public float throwDelay = 5.0f;

    // The net's position
    public Transform net;

    // Net's height
    private float netHeight = 2.8f;

    // The distance between the player and the net
    public float netDistance;

    private IEnumerator ThrowBallCoroutine()
    {
        // Set holdingBall to false
        holdingBall = false;

        yield return new WaitForSeconds(throwDelay);
        
        // Strength needed for the throw
        throwForce = StrengthOfThrow();
        Debug.LogError("throwforce"+throwForce);
        Debug.LogError("ball mass" + ballRB.mass);
        // Release the ball
        ball.transform.parent = null;

        // Apply a force to the ball
        ball.GetComponent<Rigidbody>().AddForce(throwPoint.forward*throwForce*ballRB.mass, ForceMode.Impulse);

    }

    

    // Adapts the Orientation of the throw to the height of the net
    private void ThrowPointOrientation()
    {
        // Retrieves the distance to the net 
        netDistance = transform.position.x - net.position.x;

        // Calculate the right angle to throw the ball over the net 
        float angle = Mathf.Atan((netHeight-throwPoint.position.y)/netDistance);
        angle = Mathf.Clamp(angle, 0.0f, Mathf.PI / 2.0f); // Mathf.PI reduces the angle to max 90° otherwise the ball wouldn't pass the net

        // Set the throwPoint rotation based on the calculated angle
        throwPoint.rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, transform.rotation.eulerAngles.y, 0.0f);
    }

    // Adapts the strength needed to throw the ball over the net
    private float StrengthOfThrow()
    {
        float distanceToNet = Mathf.Abs(throwPoint.position.x - net.transform.position.x); // distance to the net

        float angle = Mathf.Deg2Rad * throwPoint.eulerAngles.x; // angle of the throw in radians

        float requiredVelocity = Mathf.Sqrt((distanceToNet * Physics.gravity.magnitude) / Mathf.Abs(Mathf.Sin(2 * angle) - 2 * netHeight / distanceToNet)); // velocity required to pass the ball over the net
    
        float requiredForce = requiredVelocity / Time.fixedDeltaTime; // force required to achieve the necessary velocity

        return requiredForce;
    }
}
