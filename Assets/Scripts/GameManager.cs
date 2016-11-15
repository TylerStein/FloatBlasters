using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum PlanetType { SUN, HOT, NORM, COLD, GAS };

[System.Serializable]
public struct PlanetInfo {
    public PlanetInfo(Sprite pSprite, Sprite bSprite, PlanetType typ, float mass, float threshMin, float threshMax)
    {
        planetSprite = pSprite;
        blurSprite = bSprite;
        type = typ;
        massMultiplier = mass;
        thresholdMin = threshMin;
        thresholdMax = threshMax;
    }

    public Sprite blurSprite;
    public Sprite planetSprite;
    public PlanetType type;
    public float massMultiplier;

    [Range(1, 100)]
    public float thresholdMin;

    [Range(1, 100)]
    public float thresholdMax;

    //Static function for getting a planet by type
    public static PlanetInfo getPlanet(PlanetType searchType, List<PlanetInfo> inList)
    {
        for(int i = 0; i < inList.Count; ++i)
        {
            if(inList[i].type == searchType)
            {
                return inList[i];
            }
        }

        Debug.Log("Attempted to find planet in list, none with searchtype found!");
        return new PlanetInfo(null, null, PlanetType.NORM, 0, 0, 0);
    }
}


//Struct for SolarSystem settings
public struct SolarSystemSettings
{
    public SolarSystemSettings(int pCount, int sSize,List<PlanetInfo> planets)
    {
        planetCount = pCount;
        systemSize = sSize;
        potentialPlanets = planets;
    }

    public int planetCount;
    public int systemSize;

    public List<PlanetInfo> potentialPlanets;
}

public class GameManager : MonoBehaviour {

    //Editor-Ready Variables
    public int planetCount;
    public int systemSize;

    //List of physics-affected bodies in the scene (Players, enemies, etc.)
    public List<Transform> bodyList;

    //List of starfield effect scripts
    public List<Starfield> starfieldList;

    //List of sprites and planet types associated with them
    public List<PlanetInfo> potentialPlanets;

    //Private managed variables
    private SolarSystem systemManager;
    private SolarSystemSettings settings;
    private AlienMaster alienMaster;

    //Keep ref to the player(s) script(s)
    private PlayerControl playerControl;

    //Singleton instance and getter
    private static GameManager instance = null;

    void Awake()
    {
        //On awake, check if the instance is set
        if(GameManager.instance == null)
        {
            //No instance, set this as the static instance
            GameManager.instance = this;
        }
        //Check if an instance exists that is not this one
        else if(GameManager.instance != this)
        {
            //Destroy this instance, for there can only be one!
            Destroy(gameObject);
        }
    }

	void Start () {

        if(planetCount <= 0)
        {
            Debug.LogWarning("None or invalid number of planets attempting to be generated! Must be a value greater than 0!");
        }

        if(systemSize <= 0)
        {
            Debug.LogWarning("System size must be greater than 0!");
        }

        //Find the player object
        GameObject player0 = GameObject.FindGameObjectWithTag("Player");
        bodyList.Add(player0.transform);
        playerControl = player0.GetComponent<PlayerControl>();

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Alien"))
        {
            bodyList.Add(g.transform);
        }
       

        //Fill out the settings with editor incoming info & the created dictionary
        settings = new SolarSystemSettings(planetCount, systemSize, potentialPlanets);

        //Pass the settings on to the solar system manager
        systemManager = new SolarSystem(settings);

        //Pass the affected bodies to the SolarSystem's list
        systemManager.setAffectedBodies(bodyList);

        //Create the alien master
        alienMaster = new AlienMaster(systemManager, this);
        alienMaster.Start();
	}
	
	void Update () {
        //Call the system manager's update to move planets
        systemManager.UpdatePlanets(Time.deltaTime);

        alienMaster.Update();

        //Give the starfield the player velocity info
        foreach(Starfield s in starfieldList) { s.moveField(playerControl.getVelocity()); }
       
	}
}
