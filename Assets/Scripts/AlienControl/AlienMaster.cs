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

    public AlienMaster(SolarSystem sys, GameManager mng) { system = sys; manager = mng; }

    // Use this for initialization
    public void Start () {
        //Retreive player1's PlayerControl script
        player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

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
            GameObject alien = GameObject.Instantiate(ResourceManager.instance.prefab_alien_interceptor, system.getRandomLocationNearTarget(player1.transform.position, 20, 50), Quaternion.identity, null) as GameObject;
            aliens[i] = alien.GetComponent<AlienControl>();
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
