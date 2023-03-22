using UnityEngine;

public class Bot : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    // The Bot's ally
    public GameObject ally;

    // Bot RigidBody
    private Rigidbody botRB;

    // Ball RigidBody
    private Rigidbody ballRB;

    // Ally RigidBody
    private Rigidbody allyRB;

    // The player's zone
    public Transform zone;

    // nb of touches in friendly rally
    public int nbOfTouch = 0;

    // All Scripts called
    // GoToBall script
    [HideInInspector] public GoToBall goToBall;
    // VolleyBall script
    [HideInInspector] public VolleyBall volleyBall;
    // BumpBall script
    [HideInInspector] public BumpBall bumpBall;
    // BumpBall script
    [HideInInspector] public GrabBall grabBall;
    // BumpBall script
    [HideInInspector] public SendOverNet sendOverNet;


    // Awake is only called once
    private void Awake()
    {
        // Get Components
        botRB = GetComponent<Rigidbody>();
        ballRB = ball.GetComponent<Rigidbody>();
        allyRB = ally.GetComponent<Rigidbody>();
        goToBall = GetComponent<GoToBall>();
        volleyBall = GetComponent<VolleyBall>();
        bumpBall = GetComponent<BumpBall>();
        grabBall = GetComponent<GrabBall>();
        sendOverNet = GetComponent<SendOverNet>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    private void ManageBot()
    {
        // Ball in bot's zone and on ground
        if (BallOnGround() && BallStopped() && goToBall.BallInZone())
        {
            goToBall.MoveBotBallStopped();
            grabBall.AttachBall();
            sendOverNet.ThrowPointOrientation();
            grabBall.ReleaseBall();
            sendOverNet.SendOver();
        }
        // Ball in air, creating exchange
        if (!BallOnGround() && LandingPointInZone())
        {
            if (nbOfTouch == 0 && nbOfTouch == 1)
            {
                if (IAmCloser())
                {
                    goToBall.MoveBotBallInAir();
                    bumpBall.BumpToAlly();
                }
            }
            if (nbOfTouch == 2)
            {
                //Attack()
                sendOverNet.SendOver();
            }
        }
    }


    private bool BallOnGround()
    {
        float ballSize = ball.transform.localScale.y;
        if (ballRB.position.y <= ballSize)
            return true;
        return false;
    }

    private bool BallStopped()
    {
        if (ballRB.velocity.y <= 0.1)
            return true;
        return false;
    }

    private bool LandingPointInZone()
    {
        float x = volleyBall.BallLandingPoint().x;
        if ((x > 0 && zone.position.x > 0) || (x < 0 && zone.position.x < 0))
            return true;
        return false;
    }

    // Test to see who is closer to the ball landing point
    private bool IAmCloser()
    {
        // Landing Point position
        float x = volleyBall.BallLandingPoint().x;
        float z = volleyBall.BallLandingPoint().z;
        // Bot position
        float botX = botRB.position.x;
        float botZ = botRB.position.z;
        // Ally position
        float allyX = allyRB.position.x;
        float allyZ = allyRB.position.z;

        // Distances to Landing Point
        float allyDistanceToLandingPoint = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(allyX - x), 2) + Mathf.Pow(Mathf.Abs(allyZ - z), 2));
        float botDistanceToLandingPoint = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(botX - x), 2) + Mathf.Pow(Mathf.Abs(botZ - z), 2));

        if (botDistanceToLandingPoint <= allyDistanceToLandingPoint)
            return true;
        return false;
    }



}
