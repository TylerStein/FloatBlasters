﻿using UnityEngine;
using System.Collections;

//Possible types of alien ships
public enum AlienShipType { INTERCEPTOR, FIGHTER, DRONE, BOMBER };

public class ShipController : MonoBehaviour
{
    //Physics Vars
    float mass;

    //Components
    new Transform transform;
    Sprite shipSprite;
    new Collider2D collider2D;
    new Rigidbody2D rigidbody;

    //Ship Parts
    Thrusters thrusters;
    Hull hull;
    Fueltank fuel;
    Weapon weapon;

    // Use this for initialization
    void Start () {
        transform = gameObject.transform;
        collider2D = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void steerTowards(Vector2 dir)
    {

    }

    public void fireWeapon(Vector2 target)
    {

    }

    public void takeImpact(Vector2 force, Vector2 point, float dmg)
    {

    }

    public void drainFuel(Vector2 amt)
    {

    }

    public Rigidbody2D getRigidbody()
    {
        return rigidbody;
    }

    public Vector2 getVelocity()
    {
        return rigidbody.velocity;
    }
}

/*
public class ShipFactory
{
    ShipController buildEnemyShip(AlienShipType i)
    {
        ShipController enemy = new ShipController();
        return enemy;
    }

    ShipController buildPlayerShip()
    {
        ShipController player = new ShipController();
        return player;
    }
}
*/