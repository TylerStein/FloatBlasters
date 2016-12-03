using UnityEngine;
using System.Collections;

public class PlanetColony : MonoBehaviour {
    float currHP;
    float maxHP;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    ShipComponent generateReward( float v, string n)
    {
        //generate shipComponent
        ShipComponent s = new ShipComponent();
        s.name = n;
        s.value = v;

        return s;
    }


}
