using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player_jump : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource; //Hand used to swing
    public SteamVR_Action_Pose controllerPoseAction; // pose action from controller

    public float jumpHeight = 2.0f;
    public float jumpDuration = 1.0f;

    private Rigidbody rb; // Player RB
    private bool isJumping; // Indication if player is currently jumping
    private float jumpStartTime; // Time at which the jump starts
    private Vector3 jumpStartPosition; // Position of the player at the start of the jump


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        float timeOffset = Time.time - Time.fixedTime;

        // Controller Pose
        Vector3 controllerPosition = new Vector3();
        Quaternion controllerRotation = new Quaternion();
        Vector3 controllerVelocity = new Vector3();
        Vector3 controllerAngularVelocity = new Vector3();

        bool pose = controllerPoseAction.GetPoseAtTimeOffset(inputSource, timeOffset,out controllerPosition,out controllerRotation,out controllerVelocity,out controllerAngularVelocity);

        // Direction of circular motion using the rotation of the controller
        Vector3 motionDirection = controllerRotation * Vector3.up;

        // Check if the controller is moving in a circular motion and if the player is jumping
        if(controllerVelocity.magnitude > 0.5f && Vector3.Dot(controllerVelocity,motionDirection) >0.5f && !isJumping)
        {
            // Record info at beginning of jump
            isJumping = true;
            jumpStartTime = Time.time;
            jumpStartPosition = transform.position;

            // Apply vertical velocity to make player jump
            rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
        }
        if (isJumping)
        {
            // Time elapsed since start of jump
            float jumpElapsedTime = Time.time - jumpStartTime;

            // Stop jump 
            if (jumpElapsedTime >= jumpDuration)
            {
                isJumping = false;
                rb.velocity = new Vector3(rb.velocity.x, -jumpHeight, rb.velocity.z);
            }
            else
            {
                // Calculate height of jump
                float jumpHeightAtTime = jumpHeight * (1 - Mathf.Pow((jumpElapsedTime / jumpDuration) - 1, 2));

                // Calculate position of player 
                Vector3 jumpPosition = jumpStartPosition + new Vector3(0, jumpHeightAtTime, 0);

                // Change player position
                transform.position = jumpPosition;
            }
        }
    }
}
