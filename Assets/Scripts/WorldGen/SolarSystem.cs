using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SolarSystem {

    SolarSystemSettings settings;
    
    //Holds all planet gameObjects and their contained Planet scripts
    Dictionary<GameObject, Planet> planetDictionary;

    //List of potentially affected physics bodies (eg. Players, enemies)
    private Transform[] affectedBodies;

    public SolarSystem(SolarSystemSettings settings)
    {
        this.settings = settings;
        //Generate a solar system and store it in the planetDictionary
        planetDictionary = generateSystem(settings);
    }
    
    public void UpdatePlanets(float deltaTime)
    {
        foreach(GameObject g in planetDictionary.Keys)
        {
            Planet plnt = planetDictionary[g];
            plnt.updateOrbit(deltaTime);
            for(int i = 0; i < affectedBodies.Length; ++i)
            {

                //Get needed data for comparison
                Vector2 planetPos = plnt.getWorldPosition();
                float planetRadius = plnt.getRadius();
                float planetAtmosphere = plnt.getAtmosphereRange();
                Vector2 bodyPos = affectedBodies[i].position;

                //Check if body is within atmosphere distance
                Vector2 diff = planetPos - bodyPos;
                float fDiff = diff.magnitude;
                //Debug.Log("Diff: " + fDiff + " / " + (planetRadius + planetAtmosphere));
                if(fDiff <= (planetRadius + planetAtmosphere))
                {
                    //Body is within gravity range
                    Rigidbody2D body = affectedBodies[i].GetComponent<Rigidbody2D>();
                    Vector2 forceDir = diff.normalized;

                    Debug.DrawLine(planetPos, bodyPos, Color.yellow);

                    //Calculate gravity force
                    float finalGravForce = plnt.getGravityForce() / (fDiff / planetRadius);

                    //Apply the gravity force
                    body.AddForce(forceDir * finalGravForce, ForceMode2D.Force);
                }
            }
        }
    }

    public void setAffectedBodies(List<Transform> bodies)
    {
        affectedBodies = bodies.ToArray();
    }

    public void addAffectedBody(Transform body)
    {
        Transform[] final = new Transform[affectedBodies.Length + 1];
        for(int i = 0; i < affectedBodies.Length; ++i)
        {
            final[i] = affectedBodies[i];
        }
        final[final.Length - 1] = body;
        affectedBodies = final;
    }

    //Takes solar system settings struct, returns dictionary of gameObjects and their planet scripts
    public Dictionary<GameObject, Planet> generateSystem(SolarSystemSettings settings)
    {
        //Initialize the result dictionary to be filled
        Dictionary<GameObject, Planet> result = new Dictionary<GameObject, Planet>();

        //Create a sun
        GameObject tmpSun = generateSun(settings);
        Planet tmpSunScript = tmpSun.GetComponent<Planet>();
        result.Add(tmpSun, tmpSunScript);


        //For each planet in the system, figure out distance from sun and generate the planet
        float lastDist = 0;
        float avgSpacing = settings.systemSize / settings.planetCount;
        for (int i = 0; i < settings.planetCount; ++i)
        {
            //Planet distance from sun is = last planet distance + spacing
            float planetDist = lastDist + avgSpacing;
            //Store the current planet's distance as the previous for next iteration
            lastDist = planetDist;
            //Generate the planet object
            GameObject tmpPlanet = generatePlanet(settings, planetDist);
            //Get the planet script from the generated object
            Planet tmpScript = tmpPlanet.GetComponent<Planet>();
            //Add the generated planet/script to the result dictionary
            result.Add(tmpPlanet, tmpScript);
        }

        //Return the generated dictionary
        return result;
        
    }

    public GameObject generateSun(SolarSystemSettings settings)
    {
        GameObject result;


        PlanetInfo sunInfo = PlanetInfo.getPlanet(PlanetType.SUN, settings.potentialPlanets);

        result = new GameObject("Sun");
        Planet tmpPlanet = result.AddComponent<Planet>();
        tmpPlanet.Setup(0, 0, 1, PlanetType.SUN, sunInfo.planetSprite, sunInfo.blurSprite);

        result.transform.SetParent(GameObject.FindGameObjectWithTag("SolarSystem").transform);

        return result;
    }

    //Returns a GameObject containing a Planet script and an appropriate sprite
    public GameObject generatePlanet(SolarSystemSettings settings, float planetDistance)
    {
        //Initialize the result gameobject to be created
        GameObject result;

        float planetDistPercent = 100 / (settings.systemSize / planetDistance);
        List<PlanetInfo> validPlanets = new List<PlanetInfo>();
        foreach(PlanetInfo pInfo in settings.potentialPlanets)
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
        if(validPlanets.Count > 1)
        {
            //Choose from the list of valid planets
            int rnd = Random.Range(1, validPlanets.Count);
            finalPlanet = validPlanets[rnd];
        }else if(validPlanets.Count == 1)
        {
            //Only one valid planet
            finalPlanet = validPlanets[0];
        }else
        {
            Debug.Log("NO VALID PLANETS FOUND!");
            finalPlanet = new PlanetInfo(null, null, PlanetType.NORM, 0, 0, 0);
        }

        //Create the planet gameobject
        result = new GameObject("Planet_" + finalPlanet.planetSprite.name);

        //Make the planet a child of SolarSystem gameobject
        result.transform.SetParent(GameObject.FindGameObjectWithTag("SolarSystem").transform);

        //Create the script component on the gameobject
        Planet planetScript = result.AddComponent<Planet>();

        //Generate an orbit speed, decreases with further out planets
        float oSpeed = settings.systemSize / planetDistance * 0.2f;

        //Set up the planet script
        planetScript.Setup(planetDistance, oSpeed, finalPlanet.massMultiplier, finalPlanet.type, finalPlanet.planetSprite, finalPlanet.blurSprite);

        //Return the generated gameObject
        return result;
    }

    public Vector3 getRandomLocationNearTarget(Vector2 target, float minRange, float maxRange)
    {
        Vector2 res = Vector2.zero;

        bool valid = false;

        //Keep generating a position until a valid one is made
        do
        {
            //Generate a position within the system
            Vector2 rndCircle = new Vector2(minRange, minRange) + (Random.insideUnitCircle * maxRange);
            res = target + rndCircle;

            bool inPlanet = false;

            //Check if the position intersects a planet
            foreach (GameObject g in planetDictionary.Keys)
            {
                Vector2 planetPos = g.transform.position;
                float planetRadius = g.GetComponent<Planet>().getRadius() + 10;

                if (Vector2.Distance(res, planetPos) <= planetRadius)
                {
                    inPlanet = true;
                }
            }

            if (inPlanet == false)
            {
                valid = true;
            }
        } while (!valid);

        return new Vector3(res.x, res.y, 0);
    }

}