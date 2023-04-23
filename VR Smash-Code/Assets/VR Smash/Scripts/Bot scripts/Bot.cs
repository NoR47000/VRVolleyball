using System.Collections;
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

    // Bot's turn to touch the ball
    public bool myTouch = true;

    // Throw ball after delay
    private float delay = 0f;
    // DelayLimit
    private readonly float delayLimit = 2f;
    // Check if ball is being hold by the bot
    private bool serve = false;



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
    // Ally bot script
    [HideInInspector] public Bot allyScript;
    // Ally player script
    [HideInInspector] public PlayerInfo allyPlayerScript;



    // Awake is only called once
    private void Awake()
    {
        // Get Components
        botRB = GetComponent<Rigidbody>();
        getBallScript = GetComponent<GetBallScript>();
        allyRB = ally.GetComponent<Rigidbody>();

        if (ally.GetComponent<Bot>())
        {
            allyScript = ally.GetComponent<Bot>();
        }
        else
        {
            allyPlayerScript = ally.GetComponent<PlayerInfo>();
        }



        goToBall = GetComponent<GoToBall>();
        bumpBall = GetComponent<BumpBall>();
        grabBall = GetComponent<GrabBall>();
        sendOverNet = GetComponent<SendOverNet>();

        ball = getBallScript.ball;
        ballRB = getBallScript.ballRB;
        volleyBall = ball.GetComponent<VolleyBall>();
        nbOfTouch = volleyBall.numberOfTouches;
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
    {/* Comment first if for VR use */
        if (BallStopped() && goToBall.BallInZone())
        {
            // Grabs the ball and orients the throwPoint
            if (BallTouchGround() && IAmCloser())
            {
                serve = true;
                goToBall.MoveBotBallStopped();
                grabBall.AttachBall();
            }
            else
            {
                if (delay >= delayLimit && serve)
                {
                    delay = 0;
                    grabBall.ReleaseBall();
                    sendOverNet.SendOver();

                    Debug.Log("serve");

                    StartCoroutine(IsNotHoldingBall());

                }
                else
                {
                    delay += Time.deltaTime;
                }
            }
        }
        else /*end of the commenting necessary for VR use*/if (!BallTouchGround() && LandingPointInZone() && !serve)
        {
            if ((volleyBall.numberOfTouches == 0 && IAmCloser()) || volleyBall.numberOfTouches == 1 && myTouch)
            {
                if (myTouch)
                {
                    goToBall.MoveBotBallInAir();
                    // Distance from the ball
                    float distance = Vector3.Distance(transform.position, ball.transform.position) - transform.localScale.magnitude;
                    if (Mathf.Abs(distance) <= 0.1f)
                    {
                        Debug.Log("Bump" + volleyBall.numberOfTouches);
                        ballRB.velocity = Vector3.zero;
                        bumpBall.BumpToAlly();
                        volleyBall.numberOfTouches++;
                        myTouch = false;
                        if (allyScript)
                        {
                            allyScript.myTouch = true;
                        }
                        else
                        {
                            allyPlayerScript.myTouch = true;
                        }
                    }
                }
            }
            else if (volleyBall.numberOfTouches == 2 && myTouch)
            {
                Debug.Log("sendOver");

                goToBall.MoveBotBallInAir();
                ballRB.velocity = Vector3.zero;
                sendOverNet.SendOver();
                myTouch = true;
                if (allyScript)
                {
                    allyScript.myTouch = true;
                }
                else
                {
                    allyPlayerScript.myTouch = true;
                }
                volleyBall.numberOfTouches = 0;
            }
        }
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

    private IEnumerator IsNotHoldingBall()
    {
        yield return new WaitForSeconds(3f);
        serve = false;
    }

}
