using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player_jump : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource; //Hand used to swing
    public SteamVR_Action_Pose poseAction; // pose action from controller

    public SteamVR_Action_Pose pressAction; // press action from controller
    public Transform headTransform;

    public float jumpHeight = 2f;
    public float jumpDuration = 1.0f;
    private float jumpThreshold = 3f;
    public float jumpForce = 100f;

    private Rigidbody rb; // Player RB
    private bool isJumping; // Indication if player is currently jumping

    //Initialize
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isJumping = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocityLeft = poseAction.GetVelocity(SteamVR_Input_Sources.LeftHand);
        Vector3 velocityRight = poseAction.GetVelocity(SteamVR_Input_Sources.RightHand);

        // Player swinging his arms forward and upward
        // Mean of upwards velocity between both controllers
        float upVelocity = (velocityLeft.y + velocityRight.y)/2;
        // Mean of forward velocity magnitude between both controllers
        float forwardVelocity = (Mathf.Sqrt(Mathf.Pow(velocityRight.x, 2) + Mathf.Pow(velocityRight.z, 2)) + Mathf.Sqrt(Mathf.Pow(velocityLeft.x, 2) + Mathf.Pow(velocityLeft.z, 2)))/2;

        if (!isJumping && upVelocity >= jumpThreshold && forwardVelocity>= jumpThreshold)
        {
            Debug.Log("velocity" + forwardVelocity +"jumpThreshold"+jumpThreshold);
            rb.velocity = Vector3.up * jumpForce;
            isJumping = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        //Reset isJumping when player lands
        if (collision.gameObject.name=="Plane")
        {
            isJumping = false;
        }
    }
}
