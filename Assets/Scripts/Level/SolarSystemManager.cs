using UnityEngine;
using System.Collections.Generic;

public class SolarSystemManager {

    private static SolarSystemManager instance;
    private SolarSystemManager(){}

    public static SolarSystemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SolarSystemManager();
            }
            return instance;
        }
    }

    SystemSettings settings;

    //Holds all planet gameObjects and their contained Planet scripts
    Dictionary<GameObject, Planet> planetDictionary;

    //List of potentially affected physics bodies (eg. Players, enemies)
    Transform[] affectedBodies;


    void updateSystem()
    {
        UpdatePlanets();
    }

    public void setSystem (SystemSettings s)
    {
        WorldGenerator generator = new WorldGenerator();
        settings = s;
        planetDictionary = generator.genPlanets(s);
    }

    //Update the orbit of planets and get forces to apply to rigidbodies in the scene
    public void UpdatePlanets()
    {
        var enumerator = planetDictionary.GetEnumerator();
        while (enumerator.MoveNext()) {
            GameObject g = enumerator.Current.Key;

            Planet plnt = planetDictionary[g];
            plnt.updateOrbit(Time.deltaTime);
            if (affectedBodies != null)
            {
                for (int i = 0; i < affectedBodies.Length; ++i)
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
                    if (fDiff <= (planetRadius + planetAtmosphere))
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
    }

    //Set the rigidbodies that will be affected by planet gravity
    public void setAffectedBodies(List<Transform> bodies)
    {
        affectedBodies = bodies.ToArray();
    }

    //Add a rigidbody to be affected by planet gravity
    public void addAffectedBody(Transform body)
    {
        if(affectedBodies == null) { affectedBodies = new Transform[0]; }
        Transform[] final = new Transform[affectedBodies.Length + 1];
        for (int i = 0; i < affectedBodies.Length; ++i)
        {
            final[i] = affectedBodies[i];
        }
        final[final.Length - 1] = body;
        affectedBodies = final;
    }

    //Get a random location that isn't inside a planet within the specified range from the target
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

    public void Cleanup()
    {
        instance = null;
    }
}
