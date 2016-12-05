using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

    public Starfield[] stars;
    public Vector2 moveRate;

    public Transform earthSprite;
    public float rotRate;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Starfield s in stars)
        {
            s.moveField(moveRate);
        }

        earthSprite.Rotate(Vector3.forward, Time.deltaTime * rotRate);
    }

    public void StartSinglePlayerGame()
    {

    }

    public void StartMultiPlayerGame()
    {

    }

    public void Quit()
    {
        
    }
}
