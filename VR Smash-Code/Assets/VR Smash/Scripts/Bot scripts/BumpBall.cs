using UnityEngine;

public class BumpBall : MonoBehaviour
{
    //get ball rigidbody
    private Rigidbody ballRB;

    // Find Bot Ally 
    private GameObject ally;

    // Ally position
    private Transform allyTransform;

    // GetBall script to get ball GameObject
    [HideInInspector] public GetBallScript getBallScript;
    // Bot script to get Ally
    [HideInInspector] public Bot botScript;

    // Bumps Height
    public float bumpHeight = 4.5f;

    private void Awake()
    {
        getBallScript = GetComponent<GetBallScript>();
        botScript = GetComponent<Bot>();
        ally = botScript.ally;
        allyTransform = ally.GetComponent<Transform>();
        ballRB = getBallScript.ballRB;
    }

    // Bump ball to ally
    public void BumpToAlly()
    {
        Vector3 throwVelocity = CalculateThrowVelocity();
        ballRB.velocity = throwVelocity;
    }

    // Calculate the force needed to send the ball to the ally with a height of bumpHeight
    private Vector3 CalculateThrowVelocity()
    {
        Vector3 targetDir = allyTransform.position - ballRB.position;
        float horizontalDist = new Vector3(targetDir.x, 0, targetDir.z).magnitude;
        float verticalDist = targetDir.y;

        float gravity = Physics.gravity.magnitude;
        float angle = Mathf.Atan((verticalDist + bumpHeight) / horizontalDist);
        float speed = Mathf.Sqrt((horizontalDist * gravity) / Mathf.Sin(2 * angle));

        Vector3 direction = new Vector3(targetDir.x, 0, targetDir.z).normalized;
        Vector3 upwardVelocity = Mathf.Sin(angle) * speed * Vector3.up;
        Vector3 forwardVelocity = direction * speed * Mathf.Cos(angle);

        return upwardVelocity + forwardVelocity;
    }


}
