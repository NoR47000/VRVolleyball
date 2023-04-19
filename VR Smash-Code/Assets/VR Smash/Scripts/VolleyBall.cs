using System.Collections;
using UnityEngine;
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

    public int numberOfTouches = 0;



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
        if (transform.position.y > groundThreshold)
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

        // Ball speed
        float v0x = rb.velocity.x;
        float v0y = rb.velocity.y;
        float v0z = rb.velocity.z;
        // Ball position
        float x0 = rb.position.x;
        float y0 = rb.position.y;
        float z0 = rb.position.z;
        // Gravity constant
        float g = 9.81f;

        Vector3 landingPoint = new Vector3(x0, 0, z0);

        if (rb.velocity.magnitude >= 0.5f)
        {
            if (Mathf.Pow(v0y, 2) - 2 * g * y0 >= 0)
            {
                float t = (-v0y + Mathf.Sqrt(Mathf.Pow(v0y, 2) - 2 * g * y0)) / g;
                landingPoint.x = x0 + v0x * t + 1 / 2 * g * Mathf.Pow(t, 2);
                landingPoint.y = transform.localScale.y / 2;
                landingPoint.z = z0 + v0z * t + 1 / 2 * g * Mathf.Pow(t, 2);
            }
        }
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
