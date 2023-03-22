using UnityEngine;

public class BumpBall : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    //get ball rigidbody
    private Rigidbody ballRB;

    // Ally position
    private Transform allyTransform;

    // Bumps Height
    private float bumpHeight = 3.0f;

    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }

    // Bump ball to ally
    public void BumpToAlly()
    {

    }

    // Calculate the force needed to send the ball to the ally with a height of bumpHeight
    private Vector3 bumpForce()
    {
        return Vector3.zero;
    }


}
