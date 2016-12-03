using UnityEngine;

public class PlayerManager {

    bool isMultiplayer;
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
    public void Init(bool multiplayer){
        isMultiplayer = multiplayer;
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
            players[i] = Object.Instantiate(ResourceFinder.Instance.GetPrefab("Player")) as GameObject;
        }
    }
}
