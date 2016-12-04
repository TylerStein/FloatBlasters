using UnityEngine;
using System.Collections;

public class StarfieldHandler : MonoBehaviour {

    private Starfield[] stars;
    private Vector3 lastPos;

	// Use this for initialization
	void Start () {
        stars = GetComponentsInChildren<Starfield>();
        if(stars.Length == 0)
        {
            Debug.Log("@StarfieldHandler has an empty Starfield array");

        }

        lastPos = new Vector2(transform.position.x, transform.position.y);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 currentPos = transform.position;
        Vector3 diff = lastPos - currentPos;

        foreach (Starfield s in stars)
        {
            s.moveField(diff);
        }

        lastPos = currentPos;
    }
}
