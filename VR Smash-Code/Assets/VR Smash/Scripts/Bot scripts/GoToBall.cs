using UnityEngine;

public class GoToBall : MonoBehaviour
{
    // The ball object
    private GameObject ball;

    // Ball RigidBody
    private Rigidbody ballRB;

    // The player's zone
    public Transform zone;

    // Get GrabThrowBall script for grabDistance
    public GrabBall grabBall;

    private float grabDistance;

    // Player's Move speed
    private float moveSpeed = 10.0f;

    // VolleyBall script
    [HideInInspector] public VolleyBall volleyBallScript;
    // GetBall script to get ball GameObject
    [HideInInspector] public GetBallScript getBallScript;

    private void Awake()
    {
        getBallScript = GetComponent<GetBallScript>();

        ball = getBallScript.ball;
        ballRB = getBallScript.ballRB;
        volleyBallScript = ball.GetComponent<VolleyBall>();

        grabBall = GetComponent<GrabBall>();
        grabDistance = grabBall.grabDistance;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        //MovePlayer();
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
    public void MovePlayer()
    {
        // Get the ball position on the x,z plain
        Vector3 ballPositionProjected = new Vector3(ball.transform.position.x, 0, ball.transform.position.z);

        // Get player position in x,z plain
        Vector3 positionProjected = new Vector3(transform.position.x, 0, transform.position.z);

        // Get distance to ball
        float distanceToBall = Vector3.Distance(ballPositionProjected, positionProjected);

        if (BallInZone() && grabDistance <= (distanceToBall + 0.1) && ballRB.velocity == Vector3.zero)
        {
            // Calculate the new position the player will have
            Vector3 movePlayer = Vector3.MoveTowards(transform.position, ball.transform.position, Time.deltaTime * moveSpeed);
            movePlayer = new Vector3(movePlayer.x, transform.position.y, movePlayer.z);
            transform.position = movePlayer;
        }
    }

    // Moves the bot when the ball is stopped on the ground
    public void MoveBotBallStopped()
    {
        // Get the ball position on the x,z plain
        Vector3 ballPositionProjected = new Vector3(ball.transform.position.x, 0, ball.transform.position.z);

        // Get player position in x,z plain
        Vector3 positionProjected = new Vector3(transform.position.x, 0, transform.position.z);

        // Get distance to ball
        float distanceToBall = Vector3.Distance(ballPositionProjected, positionProjected);

        if (BallInZone() && grabDistance <= (distanceToBall + 0.1))
        {
            // Calculate the new position the player will have
            Vector3 movePlayer = Vector3.MoveTowards(transform.position, ball.transform.position, Time.deltaTime * moveSpeed);
            movePlayer = new Vector3(movePlayer.x, transform.position.y, movePlayer.z);
            transform.position = movePlayer;
        }
    }

    // Moves the bot when the ball is in the air
    public void MoveBotBallInAir()
    {
        Vector3 ballLandingPoint = volleyBallScript.BallLandingPoint();

        // Get player position in x,z plain
        Vector3 positionProjected = new Vector3(transform.position.x, 0, transform.position.z);

        // Get distance to ball
        float distanceToBall = Vector3.Distance(ballLandingPoint, positionProjected);

        Debug.Log("MoveBall");

        if (grabDistance <= (distanceToBall + 0.1))
        {
            // Calculate the new position the player will have
            Vector3 movePlayer = Vector3.MoveTowards(transform.position, ballLandingPoint, Time.deltaTime * moveSpeed);
            movePlayer = new Vector3(movePlayer.x, transform.position.y, movePlayer.z);
            transform.position = movePlayer;
        }
    }
}
