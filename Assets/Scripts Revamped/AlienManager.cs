using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlienManager {
    //Singleton handling
    private static AlienManager instance;
    private AlienManager() { }

    public static AlienManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AlienManager();
            }
            return instance;
        }
    }

    //Instance vars
    List<AlienSquad> squadList;

    public void Setup()
    {

    }

    public void Start()
    {

    }

    public void Update()
    {

    }
}