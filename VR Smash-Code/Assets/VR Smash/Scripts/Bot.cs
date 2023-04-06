using UnityEngine;

public class Bot : MonoBehaviour
{
    //Ball GameObject
    private GameObject ball;

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
    // Throw ball after delay
    private float delay = 0f;
    // DelayLimit
    private float delayLimit = 4f;


    // All Scripts called
    // GetBall script
    [HideInInspector] public GetBallScript getBallScript;
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
        getBallScript = GetComponent<GetBallScript>();
        allyRB = ally.GetComponent<Rigidbody>();
        goToBall = GetComponent<GoToBall>();
        bumpBall = GetComponent<BumpBall>();
        grabBall = GetComponent<GrabBall>();
        sendOverNet = GetComponent<SendOverNet>();

        ball = getBallScript.ball;
        ballRB = getBallScript.ballRB;
        volleyBall = ball.GetComponent<VolleyBall>();
    }

    // Update is called once per frame
    void Update()
    {
        sendOverNet.ThrowPointOrientation();
    }
    private void FixedUpdate()
    {
        ManageBot();
    }

    private void ManageBot()
    {
        if (BallStopped() && goToBall.BallInZone())
        {
            // Grabs the ball and orients the throwPoint
            if (BallTouchGround() && IAmCloser())
            {
                goToBall.MoveBotBallStopped();
                grabBall.AttachBall();
            }
            else
            {
                if (delay > delayLimit)
                {
                    grabBall.ReleaseBall();
                    sendOverNet.SendOver();
                    delay = 0;
                }
                else
                {
                    delay += Time.deltaTime;
                    return;
                }
            }
        }


        //if (grabBall.holdingBall && delay < delayLimit)
        //{
        //    delay += Time.deltaTime;
        //    return;
        //}
        //// Ball in bot's zone and on ground
        //if (BallOnGround() && BallStopped() && goToBall.BallInZone())
        //{
        //    goToBall.MoveBotBallStopped();
        //    grabBall.AttachBall();
        //    sendOverNet.ThrowPointOrientation();
        //    Thread.Sleep(3000);
        //    grabBall.ReleaseBall();
        //    sendOverNet.SendOver();
        //}
        //// Ball in air, creating exchange
        //if (!BallOnGround() && LandingPointInZone())
        //{
        //    if (nbOfTouch == 0 && nbOfTouch == 1)
        //    {
        //        if (IAmCloser())
        //        {
        //            goToBall.MoveBotBallInAir();
        //            bumpBall.BumpToAlly();
        //        }
        //    }
        //    if (nbOfTouch == 2)
        //    {
        //        //Attack()
        //        sendOverNet.SendOver();
        //    }
        //}
    }


    private bool BallTouchGround()
    {
        float ballSize = ball.transform.localScale.y;
        if (ballRB.position.y <= ballSize)
            return true;
        return false;
    }

    // Test if ball is stopped
    private bool BallStopped()
    {
        if (ballRB.velocity.magnitude < 0.01)
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
        Vector3 landingPoint = volleyBall.BallLandingPoint();
        // Landing Point position
        float x = landingPoint.x;
        float z = landingPoint.z;


        // Bot position
        float botX = botRB.position.x;
        float botZ = botRB.position.z;
        // Ally position
        float allyX = allyRB.position.x;
        float allyZ = allyRB.position.z;


        // Distances to Landing Point
        double allyDistanceToLandingPoint = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(allyX - x), 2) + Mathf.Pow(Mathf.Abs(allyZ - z), 2));
        double botDistanceToLandingPoint = Mathf.Sqrt((botX - x) * (botX - x) + (botZ - z) * (botZ - z));

        if (botDistanceToLandingPoint <= allyDistanceToLandingPoint)
            return true;
        return false;
    }



}
