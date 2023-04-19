using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Player_jump : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource; //Hand used to swing
    public SteamVR_Action_Pose controllerPoseAction; // pose action from controller
    public Transform headTransform;

    public float jumpHeight = 2.0f;
    public float jumpDuration = 1.0f;
    public float jumpThreshold = 0.3f;
    public float jumpForce = 2.0f;

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
    void FixedUpdate()
    {
        float timeOffset = Time.time - Time.fixedTime;

        // Controller Pose
        Vector3 controllerPosition = new Vector3();
        Quaternion controllerRotation = new Quaternion();
        Vector3 controllerVelocity = new Vector3();
        Vector3 controllerAngularVelocity = new Vector3();

        bool pose = controllerPoseAction.GetPoseAtTimeOffset(inputSource, timeOffset,out controllerPosition,out controllerRotation,out controllerVelocity,out controllerAngularVelocity);

        //Debug.Log("controller velocity: " + controllerVelocity.y);
        //Debug.Log("bool isjump" + isJumping);
        if (!isJumping && controllerVelocity.y>jumpThreshold)
        {
            Debug.Log("Stqrt jump");
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            transform.Translate(Vector3.up * jumpForce);
            isJumping = true;
            //StartCoroutine(PlayerFall());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        //Reset isJumping when player lands
        if (collision.gameObject.CompareTag("Plane"))
        {
            Debug.Log("Collision"); // doesn't enter if 
            isJumping = false;
        }
    }

    private IEnumerator PlayerFall()
    {
        yield return new WaitForSeconds(jumpDuration);
        transform.Translate(-Vector3.up *9.81f);
        isJumping = false;
    }
}
