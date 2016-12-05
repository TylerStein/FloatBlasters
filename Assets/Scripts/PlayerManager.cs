using UnityEngine;

public class PlayerManager {

    public bool isMultiplayer;
    GameObject[] players;

    private static PlayerManager instance;
    private PlayerManager() { }

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerManager();
            }
            return instance;
        }
    }

    //Initialize the players
    public void Init(){
        if (isMultiplayer)
        {
            spawnPlayers(2);
        }else
        {
            spawnPlayers(1);
        }
    }

    //Spawn the desired number of players
    private void spawnPlayers(int count) { 
        players = new GameObject[count];
        for(int i = 0; i < count; ++i)
        {
            players[i] = Object.Instantiate(ResourceFinder.Instance.GetPrefab("Player"), new Vector3(0, 100, 0), Quaternion.identity) as GameObject;
            SolarSystemManager.Instance.addAffectedBody(players[i].transform);
        }
    }
}
