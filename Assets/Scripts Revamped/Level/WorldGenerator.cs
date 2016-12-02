using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct SystemSettings
{
    public SystemSettings(int pCount, int sSize, List<PlanetInfo> planets)
    {
        planetCount = pCount;
        systemSize = sSize;
        potentialPlanets = planets;
    }

    public int planetCount;
    public int systemSize;

    public List<PlanetInfo> potentialPlanets;
}

public class WorldGenerator : MonoBehaviour {


    List<PlanetInfo> genPlanets(SystemSettings s)
    {
         List<PlanetInfo> npi = new List<PlanetInfo>();
        return npi;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
