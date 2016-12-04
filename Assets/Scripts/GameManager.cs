using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    //Editor-Ready Variables
    public int planetCount;
    public int systemSize;
    public bool bMultiplayer;

    //List of sprites and planet types associated with them
    public List<PlanetInfo> potentialPlanets;

    //Private managed variables
    private SolarSystemManager systemManager;
    private SystemSettings settings;
    private AlienManager alienManager;
    private PlayerManager playerManager;

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

        Start();
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
        alienManager = AlienManager.Instance;

        //Grab and initialize the player manager
        playerManager = PlayerManager.Instance;
        playerManager.Init(false);

        Instantiate(ResourceFinder.Instance.GetPrefab("StarCamera"));

        //Fill out the settings with editor incoming info & the created dictionary
        settings = new SystemSettings(planetCount, systemSize, potentialPlanets);

        //Pass the settings on to the solar system manager
        systemManager.setSystem(settings);

        //Create the alien master
        alienManager.Setup();
    }

    void Update()
    {
        //Call the system manager's update to move planets
        systemManager.UpdatePlanets();

        //Call the alien manager's update to move AI
        alienManager.Update();
    }
}
