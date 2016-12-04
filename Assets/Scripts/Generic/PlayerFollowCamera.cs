using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour {

    public Transform playerRef;

    public float zOffset = -10;

    public bool doLerp = false;
    public float lerpSpeed = 1;

    private Vector3 lastPos;
    bool foundPlayer;

	// Use this for initialization
	void Start () {
        try
        {
            playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        }catch
        {
            foundPlayer = false;
        }
        if(playerRef == null) { foundPlayer = false; }
        else { foundPlayer = true; }
	}
	
	// Update is called once per frame
	void Update () {
        if (foundPlayer == false)
        {
            try
            {
                playerRef = GameObject.FindGameObjectWithTag("Player").transform;
            }
            catch
            {
                foundPlayer = false;
            }
            if (playerRef != null) { foundPlayer = true; }
        }
        else
        {

            Vector3 targetPos = new Vector3(playerRef.position.x, playerRef.position.y, zOffset);

            if (doLerp)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
            }
            else
            {
                //Set the position to the player's XY and the specified Z
                transform.position = targetPos;
            }
        }
	}
}
