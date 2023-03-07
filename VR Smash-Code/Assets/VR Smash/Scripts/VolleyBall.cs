using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBall : MonoBehaviour
{
    private GrabThrowBall grabThrowBallScript;

    private void Start()
    {
        grabThrowBallScript = GetComponent<GrabThrowBall>();
    }
    void OnCollisionEnter(Collision collision)
    {
        // Simulates interaction with external world
        if (collision.gameObject.CompareTag("AlliedSide")||collision.gameObject.CompareTag("EnemySide"))
        {
            grabThrowBallScript.hasBeenThrown = false;
        }
    }
}
