using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// Link to video
//https://www.youtube.com/watch?v=nodALlM_IKY

//public class PickUpHand : MonoBehaviour
//{
//    public float distToPickup = 0.3f;
//    bool handClosed = false;
//    public LayerMask pickupLayer;

//    public SteamVR_Input_Sources handSource = SteamVR_Input_Sources.LeftHand;

//    RigidBody holdingTarget;

//    void FixedUpdate()
//    {
//        if (SteamVR_Actions.shooter_Grab.GetState(handSource))
//            handClosed = true;
//        else
//            handClosed = false;

//        if (!handClosed)
//        {
//            Collider[] colliders = Physics.OverlapSphere(transform.position, distToPickup, pickupLayer);
//            if (colliders.Length > 0)
//                holdingTarget = colliders[0].transform.root.GetComponent<RigidBody>();
//            else
//                holdingTarget = null;
//        }
//        else
//        {
//            if (holdingTarget)
//            {
//                //adjust velocity to move to hand
//                holdingTarget.velocity = (transform.position - holdingTarget.transform.position) / Time.fixedDeltaTime;

//                //adjust angular velocity to rotate to hand
//                holdingTarget.maxAngularVelocity = 20;
//                Quaternion deltaRot = transform.rotation * Quaternion.Inverse(holdingTarget.transform.rotation);
//                Vector3 eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x), Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
//                eulerRot *= 0.95f;
//                eulerRot *= Mathf.Deg2Rad;
//                holdingTarget.angularVelocity = eulerRot / Time.fixedDeltaTime;
//            }
//        }
//    }
//}
