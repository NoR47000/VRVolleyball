using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player_jump : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource; //Hand used to swing
    public SteamVR_Action_Pose poseAction; // pose action from controller
    public Transform headTransform;

    public float jumpHeight = 2f;
    public float jumpDuration = 1.0f;
    public float jumpThreshold = 1.5f;
    public float jumpForce = 100f;

    private Rigidbody rb; // Player RB
    private bool isJumping; // Indication if player is currently jumping


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        isJumping = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity = poseAction.GetVelocity(inputSource);

        if (!isJumping && velocity.y>jumpThreshold)
        {
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
