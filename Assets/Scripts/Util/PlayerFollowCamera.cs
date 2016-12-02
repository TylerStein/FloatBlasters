using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour {

    public Transform playerRef;

    public float zOffset = -10;
    public float followSpeed = 2;

	// Use this for initialization
	void Start () {
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 targetPos = new Vector3(playerRef.position.x, playerRef.position.y, zOffset);
        //Vector3 movement = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
        transform.position = targetPos;
	}
}
