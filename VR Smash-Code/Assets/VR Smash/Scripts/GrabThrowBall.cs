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
    public bool holdingBall = false;

    // Has the ball been Thrown ?
    public bool hasBeenThrown = false;

    private void Awake()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Orientation of throwPoint
        ThrowPointOrientation();

        if (!holdingBall && ballRB.velocity.magnitude <= 0 && Vector3.Distance(ball.transform.position, transform.position) <= grabDistance)
        {
            
            AttachBall();

            // Set holdingBall to true
            holdingBall = true;

        }

        // If the player is holding the ball
        if(holdingBall && !hasBeenThrown)
        {
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
    private float netHeight = 2.43f;

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
        ball.GetComponent<Rigidbody>().AddForce(throwPoint.forward*throwForce, ForceMode.Impulse);

    }

    

    // Adapts the Orientation of the throw to the height of the net
    private void ThrowPointOrientation()
    {
        // Retrieves the distance to the net 
        netDistance = Mathf.Abs(transform.position.x - net.position.x);

        // Calculate the right angle to throw the ball over the net 
        float angle = Mathf.Atan(netHeight/netDistance);

        // Set the throwPoint rotation based on the calculated angle
        throwPoint.rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, transform.rotation.eulerAngles.y, 0.0f);
    }

    // Adapts the strength needed to throw the ball over the net
    private float StrengthOfThrow()
    {
        float randomHeight = UnityEngine.Random.Range(0.1f,2.5f); // Generates a random float to have a changing height to go over the net
        float randomDistance = UnityEngine.Random.Range(0.0f,1.0f); // Generates a random float to have a changing distance from the net
        float gravityConstant = 9.81f;
        float h = (netHeight - throwPoint.position.y) + randomHeight;
        float d =  2*Mathf.Abs(net.position.x-throwPoint.position.x)+randomDistance;// approximation of the Horizontal distance between the starting point and the impact point on the ground 
        float alpha = Mathf.Atan(2*h / d); // Angle between the trajectory of the ball wanted and the horizon
        float requiredForce = ballRB.mass * gravityConstant * Mathf.Sin(alpha);

        return requiredForce;
    }

    private void AttachBall()
    {
        // Set the ball's position and rotation to match the player's throwing point
        ball.transform.position = throwPoint.position;
        ball.transform.rotation = throwPoint.rotation;

        // Make the ball a child of the player object
        ball.transform.parent = transform;
    }
}
