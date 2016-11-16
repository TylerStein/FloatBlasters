using UnityEngine;
using System.Collections;

public class AlienMaster {
    //Player refs
    public PlayerControl player1;
    public PlayerControl player2;

    //Solar system ref
    public SolarSystem system;

    //GameManager ref
    public GameManager manager;

    //Squads in play
    private SquadCommander[] liveSquads;

    //Alien generator
    private AlienFactory alienFactory;

    public AlienMaster(SolarSystem sys, GameManager mng) { system = sys; manager = mng; }

    // Use this for initialization
    public void Start () {
        //Retreive player1's PlayerControl script
        player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        alienFactory = new AlienFactory(system);
        spawnSquad(SquadType.PERSUIT, 3);
    }

    void spawnSquad(SquadType type, int count)
    {
        //Create liveSquads if null
        if(liveSquads == null) { liveSquads = new SquadCommander[0]; }

        //Add a squad to the list
        SquadCommander[] backup = new SquadCommander[liveSquads.Length];
        liveSquads.CopyTo(backup, 0);
        liveSquads = new SquadCommander[backup.Length + 1];
        for(int i = 0; i < backup.Length; ++i)
        {
            liveSquads[i] = backup[i];
        }

        //Create a list of aliens for the squad
        AlienControl[] aliens = new AlienControl[count];
        for(int i = 0; i < count; ++i)
        {
            aliens[i] = alienFactory.spawnAlien(ShipType.Interceptor, player1.transform);
        }


        //Add the squad
        liveSquads[liveSquads.Length - 1] = new PersuitSquad(aliens, player1.transform, system);

    }

    // Update is called once per frame
    public void Update() {
        for (int i = 0; i < liveSquads.Length; ++i)
        {
            liveSquads[i].DoCommand();
        }
        
	}
}

public class AlienFactory{
    public SolarSystem sys;

    public AlienFactory(SolarSystem system)
    {
       sys = system;
    }

    public AlienControl spawnAlien(ShipType type, Transform targetLocation)
    {
        AlienControl ctrl;
        GameObject alien;

        //Only one type of ship for now
        switch (type)
        {
            //Create an interceptor
            case ShipType.Interceptor:
                alien = Object.Instantiate(ResourceManager.instance.prefab_alien_interceptor, sys.getRandomLocationNearTarget(targetLocation.position, 20, 50), Quaternion.identity, null) as GameObject;
                ctrl = alien.GetComponent<AlienControl>();
                return ctrl;
        }

        //Could not create ship
        return null;
    }
}
