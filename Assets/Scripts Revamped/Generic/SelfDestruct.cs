using UnityEngine;
using System.Collections;

//Class to be added to an object to destroy it if Object.Destroy() is not usable
public class SelfDestruct : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
