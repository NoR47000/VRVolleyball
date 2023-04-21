using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnBall : MonoBehaviour
{

    public GameObject ball;
    public Rigidbody ballRB;


    public SteamVR_Action_Boolean spawnBallAction;


    private void Awake()
    {
        ball = GameObject.Find("VolleyBall");
        ballRB = ball.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (spawnBallAction.GetLastStateDown(SteamVR_Input_Sources.LeftHand))
        {
            ballRB.Sleep();
            ball.transform.position = transform.position + new Vector3(0.25f,0,0.25f);
            ballRB.WakeUp();
        }
    }
}
