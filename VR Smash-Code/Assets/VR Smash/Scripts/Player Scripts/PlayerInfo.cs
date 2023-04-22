using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Bot's turn to touch the ball
    public bool myTouch = true;
    // nb of touches in friendly rally
    public int nbOfTouch;

    // The Bot's ally
    public GameObject ally;

    // Ally bot script
    [HideInInspector] public Bot allyScript;
    // VolleyBall script
    [HideInInspector] public VolleyBall volleyBall;

    private void Awake()
    {
        allyScript = ally.GetComponent<Bot>();
        volleyBall = GameObject.Find("VolleyBall").GetComponent<VolleyBall>();
        nbOfTouch = volleyBall.numberOfTouches;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Reset isJumping when player lands
        if (myTouch && collision.gameObject.name == "VolleyBall")
        {
            nbOfTouch++;
            myTouch = false;
            allyScript.myTouch = true;
        }

    }
}
