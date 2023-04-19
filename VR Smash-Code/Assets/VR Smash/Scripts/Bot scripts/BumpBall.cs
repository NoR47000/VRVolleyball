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
    //private float bumpHeight = 3.0f;

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
        Vector3 v0 = bumpForce();
        ballRB.velocity = v0;
    }

    // Calculate the force needed to send the ball to the ally with a height of bumpHeight
    private Vector3 bumpForce()
    {
        // Vector representing the speed needed at the contact time to redirect ball near the ally
        Vector3 v0;
        // Wanted arrival position near ally
        Vector3 arrivalPosition = new(allyTransform.position.x, allyTransform.position.y + 0.07f, allyTransform.position.z);
        // Position where the ball arrives near the bot
        Vector3 startPosition = ballRB.position;

        // Gravity constant 
        float g = Physics.gravity.y;

        // X,Y,Z initial axial speeds needed to reach the arrival position
        float v0x = Mathf.Sqrt(-g * startPosition.x * (arrivalPosition.x - startPosition.x / (arrivalPosition.y - startPosition.y)));
        float v0y = g * (arrivalPosition.x - 3 * startPosition.x) / (2 * v0x);
        float v0z = v0x * (arrivalPosition.z - startPosition.z) / (arrivalPosition.x - startPosition.x);

        v0 = new Vector3(v0x, v0y, v0z);

        return v0;
    }


}
