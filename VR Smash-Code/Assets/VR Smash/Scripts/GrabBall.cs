using UnityEngine;

public class GrabBall : MonoBehaviour
{
    // The ball object
    private GameObject ball;

    // The player's grab distance
    public float grabDistance = 1.0f;

    //get ball rigidbody
    private Rigidbody ballRB;

    // The player's throwing point
    public Transform throwPoint;

    // Is the player holding the ball?
    public bool holdingBall = false;

    //Fixed Joint
    public FixedJoint fixedJoint;

    // GetBall script to get ball GameObject
    [HideInInspector] public GetBallScript getBallScript;


    private void Awake()
    {
        getBallScript = GetComponent<GetBallScript>();

        ball = getBallScript.ball;
        ballRB = getBallScript.ballRB;
    }
    public void AttachBall()
    {
        if (!holdingBall && ballRB.velocity.magnitude <= 0 && Vector3.Distance(ball.transform.position, transform.position) <= grabDistance)
        {
            // Set holdingBall to true
            holdingBall = true;

            Debug.Log("Attach");
            // Set the ball's position and rotation to match the player's throwing point
            ball.transform.position = throwPoint.position;
            ball.transform.rotation = throwPoint.rotation;

            // Make the ball a child of the player object
            ball.transform.parent = throwPoint.transform;

            // Add a Fixed Joint component to the ball and connect it to the throw point
            fixedJoint = ball.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = throwPoint.GetComponent<Rigidbody>();
        }
    }

    public void ReleaseBall()
    {

        // Release the ball
        ball.transform.parent = null;
        holdingBall = false;
        // Remove the Fixed Joint component from the ball
        Destroy(fixedJoint);
    }
}
