using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

public class VolleyBall : MonoBehaviour
{
    // Movement value under which the ball is stopped
    public float stopThreshold = 0.1f;

    // Trigger delay before ball is ungrabbable
    private readonly float triggerDelay = 0.1f;

    // Height under which the code enters the if
    private readonly float groundThreshold = 0.5f;
    private Rigidbody rb;

    private bool canGrab = false;

    // Get right and left hands to act on trigger
    private MeshCollider leftHand;
    private MeshCollider rightHand;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        leftHand = GameObject.Find("LeftHand").GetComponent<Hand>().GetComponent<MeshCollider>();
        rightHand = GameObject.Find("RightHand").GetComponent<Hand>().GetComponent<MeshCollider>();
    }

    void FixedUpdate()
    {
        //Check if ball is on the ground
        bool ground = (transform.position.y < groundThreshold);

        // Stop the ball's movement if it's on the ground and it is almost stopped
        // Prevents the ball from rotating indefinitily
        if (ground && Quaternion.Angle(transform.rotation, Quaternion.identity) < stopThreshold && rb.velocity.magnitude < stopThreshold && rb.velocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Update()
    {
        if (transform.position.y > groundThreshold )
        {
            if (canGrab)
            {
                StartCoroutine(DisableGrab());
            }
        }
        else
        {
            if (!canGrab)
            {
                leftHand.isTrigger = true;
                rightHand.isTrigger = true;
                canGrab = true;
            }
        }
    }


    public Vector3 BallLandingPoint()
    {

        Vector3 landingPoint = new Vector3(0, 0, 0);
        // Ball speed
        Vector3 velocity = rb.velocity;
        // Ball position
        float x0 = rb.position.x;
        float y0 = rb.position.y;
        float z0 = rb.position.z;

        if (velocity.magnitude <= 0)
        {
            landingPoint.x = x0;
        }
        else
        {
            float v0 = Mathf.Sqrt((velocity.x) * (velocity.x) + (velocity.y) * (velocity.y) + (velocity.z) * (velocity.z));

            // Angle at which it is thrown
            float alpha = Vector3.Angle(velocity, new Vector3(1, 0, 0));

            // Necessary variable
            float g = Physics.gravity.y;

            // Calculus to resolve 0 = A*x^2 + B*x + C
            float A = -g / (2 * Mathf.Cos(alpha) * v0 * v0);
            float B = -2 * x0 * A + Mathf.Tan(alpha);
            float C = A * x0 * x0 - Mathf.Tan(alpha) * x0 + y0;

            // Roots of the equation
            if (A * C > 0)
            {
                landingPoint.x = (-B + Mathf.Sqrt(4 * A * C)) / (2 * A);
            }
        }

        landingPoint.y = 0;
        landingPoint.z = z0;

        return landingPoint;
    }

    private IEnumerator DisableGrab()
    {
        yield return new WaitForSeconds(triggerDelay);
        leftHand.isTrigger = false;
        rightHand.isTrigger = false;
        canGrab = false;
    }
}
