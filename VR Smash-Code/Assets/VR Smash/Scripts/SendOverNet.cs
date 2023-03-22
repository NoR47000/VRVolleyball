using UnityEngine;

public class SendOverNet : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    //get ball rigidbody
    private Rigidbody ballRB;

    // The player's throwing point
    public Transform throwPoint;
    // The net's position
    public Transform net;

    // Net's height
    private float netHeight = 2.43f;
    // The distance between the player and the net
    public float netDistance;

    // The force to apply to the ball when it's thrown
    public float throwForce = 10.0f;

    // Awake is only called once at the beginning
    private void Awake()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendOver()
    {
        // Strength needed for the throw
        throwForce = StrengthOfThrow();
        // Apply a force to the ball
        ball.GetComponent<Rigidbody>().AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
    }

    // Adapts the Orientation of the throw to the height of the net
    public void ThrowPointOrientation()
    {
        // Retrieves the distance to the net 
        netDistance = Mathf.Abs(transform.position.x - net.position.x);

        // Calculate the right angle to throw the ball over the net 
        float zAngle = Mathf.Atan(netHeight / netDistance);

        // Set the throwPoint rotation based on the calculated angle
        throwPoint.rotation = Quaternion.Euler(-zAngle * Mathf.Rad2Deg, transform.rotation.eulerAngles.y, 0.0f);
    }

    private float StrengthOfThrow()
    {
        float randomHeight = Random.Range(0.1f, 2.5f); // Generates a random float to have a changing height to go over the net
        float gravityConstant = Physics.gravity.y;
        float h = (netHeight - throwPoint.position.y) + randomHeight;
        float d = 2 * Mathf.Abs(net.position.x - throwPoint.position.x);// approximation of the Horizontal distance between the starting point and the impact point on the ground 
        float alpha = Mathf.Atan(2 * h / d); // Angle between the trajectory of the ball wanted and the horizon
        float requiredForce = ballRB.mass * gravityConstant * Mathf.Sin(alpha);

        return requiredForce;
    }
}