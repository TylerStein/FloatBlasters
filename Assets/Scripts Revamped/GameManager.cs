using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    //Editor-Ready Variables
    public int planetCount;
    public int systemSize;
    public bool bMultiplayer;

    public GameObject playerPrefab;

    //List of physics-affected bodies in the scene (Players, enemies, etc.)
    public List<Transform> bodyList;

    //List of starfield effect scripts
    public List<Starfield> starfieldList;

    //List of sprites and planet types associated with them
    public List<PlanetInfo> potentialPlanets;

    //Private managed variables
    private SolarSystemManager systemManager;
    private SystemSettings settings;
    private AlienManager alienMaster;

    //Keep ref to the player(s) script(s)
    private PlayerShip playerControl;

    //Singleton instance and getter
    private static GameManager instance = null;

    void Awake()
    {
        //On awake, check if the instance is set
        if (GameManager.instance == null)
        {
        //No instance, set this as the static instance
        GameManager.instance = this;
        }
        //Check if an instance exists that is not this one
        else if (GameManager.instance != this)
        {
            //Destroy this instance, for there can only be one!
            Destroy(gameObject);
        }
    }

    void Start()
    {

        if (planetCount <= 0)
        {
            Debug.LogWarning("None or invalid number of planets attempting to be generated! Must be a value greater than 0!");
        }

        if (systemSize <= 0)
        {
            Debug.LogWarning("System size must be greater than 0!");
        }

        //Grab the system manager
        systemManager = SolarSystemManager.Instance;

        //Grab the alien manager
        alienMaster = AlienManager.Instance;

        GameObject player0 = Instantiate(playerPrefab, new Vector3(0, 100), Quaternion.identity) as GameObject;
        bodyList.Add(player0.transform);
        playerControl = player0.GetComponent<PlayerShip>();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Alien"))
        {
            bodyList.Add(g.transform);
        }

        //Fill out the settings with editor incoming info & the created dictionary
        settings = new SystemSettings(planetCount, systemSize, potentialPlanets);

        //Pass the settings on to the solar system manager
        systemManager.setSystem(settings);

        //Pass the affected bodies to the SolarSystem's list
        systemManager.setAffectedBodies(bodyList);

        //Build the solar system
        systemManager.generateSystem();

        //Create the alien master
        alienMaster.Setup();
        alienMaster.Start();
    }

    void Update()
    {
        //Call the system manager's update to move planets
        systemManager.UpdatePlanets(Time.deltaTime);

        alienMaster.Update();

        //Give the starfield the player velocity info
        foreach (Starfield s in starfieldList) { s.moveField(playerControl.getVelocity()); }
    }
}
