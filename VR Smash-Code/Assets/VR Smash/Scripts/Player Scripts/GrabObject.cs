using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GrabObject : MonoBehaviour
{

    public SteamVR_Action_Boolean GrabAction = null;

    private SteamVR_Behaviour_Pose Pose = null;
    private FixedJoint Joint = null;

    private Interactable CurrentInteractable = null;
    // List of the interactables in contact with object
    private List<Interactable> ContactInteractables = new List<Interactable>();



    private void Awake()
    {
        Pose = GetComponent<SteamVR_Behaviour_Pose>();
        Joint = GetComponent<FixedJoint>();
    }
    private void Update()
    {
        //Down 
        if (GrabAction.GetStateDown(Pose.inputSource))
        {
            PickUp();
        }
        //Up
        if (GrabAction.GetStateUp(Pose.inputSource))
        {
            Drop();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        ContactInteractables.Add(other.gameObject.GetComponent<Interactable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Interactable"))
            return;

        ContactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
    }

    //Pick up Object/Interactable
    public void PickUp()
    {
        //Get nearest
        CurrentInteractable = GetNearestInteractable();
        //Null Check
        if (!CurrentInteractable)
            return;
        //Already Held Check
        if (CurrentInteractable.ActiveHand != null)
            //CurrentInteractable.ActiveHand.Drop();

            //Position Interactable to controller
            CurrentInteractable.transform.position = transform.position;

        //Attach
        Rigidbody targetBody = CurrentInteractable.GetComponent<Rigidbody>();
        Joint.connectedBody = targetBody;

        //Set Active Hand
        //CurrentInteractable.ActiveHand = this;

    }

    //Drop/Let go of Object/Interactable
    public void Drop()
    {
        //Null Check
        if (!CurrentInteractable)
            return;

        //Apply velocity
        Rigidbody targetBody = CurrentInteractable.GetComponent<Rigidbody>();
        targetBody.velocity = Pose.GetVelocity();

        //Detach
        Joint.connectedBody = null;

        //Clear
        CurrentInteractable.ActiveHand = null;
        CurrentInteractable = null;
    }

    private Interactable GetNearestInteractable()
    {
        Interactable nearest = null;
        float minDistance = float.MaxValue;
        float distance = 0.0f;

        foreach (Interactable interactable in ContactInteractables)
        {
            distance = (interactable.transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = interactable;
            }
        }
        return nearest;
    }
}
