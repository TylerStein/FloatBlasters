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

public class WorldGenerator {
    public WorldGenerator() { }

    public Dictionary<GameObject, Planet> genPlanets(SystemSettings s)
    {
        Dictionary<GameObject, Planet> res = new Dictionary<GameObject, Planet>();

        GameObject systemHolder = Object.Instantiate(new GameObject("SolarSystem")) as GameObject;
        systemHolder.tag = "SolarSystem";

        float average_distance = s.systemSize / s.planetCount;
        float max_difference = average_distance / 2;
        float total_distance = average_distance;

        GameObject sun = generateSun(s);
        res.Add(sun, sun.GetComponent<Planet>());

        //Planet count minus the sun
        for(int i = 0; i < s.planetCount - 1; ++i)
        {
            total_distance += rngMod(total_distance, max_difference);
            GameObject newPlanet = generatePlanet(s, total_distance);
            res.Add(newPlanet, newPlanet.GetComponent<Planet>());
        }

        return res;
    }

    float rngMod(float original, float maxMod)
    {
        return (original + Random.Range(-maxMod, maxMod));
    }

    //Create the sun (always centered)
    GameObject generateSun(SystemSettings settings)
    {
        GameObject result;

        PlanetInfo sunInfo = PlanetInfo.getPlanet(PlanetType.SUN, settings.potentialPlanets);

        result = new GameObject("Sun");
        result.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Planet tmpPlanet = result.AddComponent<Planet>();
        tmpPlanet.Setup(sunInfo, 0, 0);
        
        result.transform.SetParent(GameObject.FindGameObjectWithTag("SolarSystem").transform);


        return result;
    }

    //Returns a GameObject containing a Planet script and an appropriate sprite
    GameObject generatePlanet(SystemSettings settings, float planetDistance)
    {
        //Initialize the result gameobject to be created
        GameObject result;

        float planetDistPercent = 100 / (settings.systemSize / planetDistance);
        List<PlanetInfo> validPlanets = new List<PlanetInfo>();
        foreach (PlanetInfo pInfo in settings.potentialPlanets)
        {
            // Debug.Log("Checking if valid planet!");
            //Found a planet matching the distance criterea
            if (planetDistPercent <= pInfo.thresholdMax && planetDistPercent >= pInfo.thresholdMin)
            {
                validPlanets.Add(pInfo);
            }
        }


        //Figure out the final planet info by choosing randomly from the list of valid planets
        PlanetInfo finalPlanet;
        if (validPlanets.Count > 1)
        {
            //Choose from the list of valid planets
            int rnd = Random.Range(1, validPlanets.Count);
            finalPlanet = validPlanets[rnd];
        }
        else if (validPlanets.Count == 1)
        {
            //Only one valid planet
            finalPlanet = validPlanets[0];
        }
        else
        {
            Debug.Log("NO VALID PLANETS FOUND!");
            finalPlanet = new PlanetInfo(null, null, PlanetType.NORM, 0, 1, 0, 1);
        }

        //Create the planet gameobject
        result = new GameObject("Planet_" + finalPlanet.planetSprite.name);
        result.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        //Make the planet a child of SolarSystem gameobject
        result.transform.SetParent(GameObject.FindGameObjectWithTag("SolarSystem").transform);

        //Create the script component on the gameobject
        Planet planetScript = result.AddComponent<Planet>();

        //Generate an orbit speed, decreases with further out planets
        float oSpeed = settings.systemSize / planetDistance * 0.2f;



        //Set up the planet script
        planetScript.Setup(finalPlanet, oSpeed, planetDistance);

        //Return the generated gameObject
        return result;
    }

}
