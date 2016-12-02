using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour {

    public Transform playerRef;

    public float zOffset = -10;
    public float followSpeed = 2;

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
        if(foundPlayer == false)
        {
            playerRef = GameObject.FindGameObjectWithTag("Player").transform;
            if(playerRef != null) { foundPlayer = true; }
        }
        //Set the position to the player's XY and the specified Z
        Vector3 targetPos = new Vector3(playerRef.position.x, playerRef.position.y, zOffset);
        transform.position = targetPos;
	}
}
