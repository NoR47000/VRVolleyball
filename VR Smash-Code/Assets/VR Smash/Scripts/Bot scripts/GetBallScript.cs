using UnityEngine;

public class GetBallScript : MonoBehaviour
{
    // The ball object
    public GameObject ball;

    //get ball rigidbody
    [HideInInspector] public Rigidbody ballRB;

    private void Awake()
    {
        ballRB = ball.GetComponent<Rigidbody>();
    }
}
