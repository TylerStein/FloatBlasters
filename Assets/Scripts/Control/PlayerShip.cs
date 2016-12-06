using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class PlayerShip : MonoBehaviour {

    int playerSlot;
    ShipController ship;
    //Component references
    public Animator animator;
    public ParticleSystem particles;
    new Rigidbody2D rigidbody;
    //Ship Vars
    float rotationVal = 5.0f;
    float fuelCost = 0.02f;
    float fuelCurrent = 100.0f;
    float fuelTotal = 100.0f;
    float armor = 0.2f;
    float currentHP = 100;
    float totalHP = 100;
    float maxSpeed = 500000;
    Vector2 force = new Vector2(500, 0);
    float forceX;
    //weapon Vars
    float cooldown = 0.2f;
    float damage = 5f;
    float dForce = 50f;
    float range = 120f;
    float timer = 0;

    int thrusterLevel;
    int engineLevel;
    int hullLevel;
    int wepLevel;

    public Image healthbar;
    public Image fuelbar;
    void Start () {
        //break apart force

        forceX = force.x;
        ship = gameObject.AddComponent<ShipController>();
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody2D>();
        thrusterLevel = 1;
        engineLevel = 1;
        hullLevel = 1;
        wepLevel = 1;
        //create components for the playerShip
        ship.defaultFitting(fuelTotal, fuelCurrent, armor, totalHP, currentHP, fuelCost, forceX, maxSpeed);
        ship.weaponFitting(cooldown,damage,dForce,range);
    }

    // Update is called once per frame
    void Update() {
        //ui stuff
        //todo set the healthbar and fuel bar and display the level of the ships attributes
        //healthbar.fillAmount = currentHP / 100;
        //fuelbar.fillAmount = fuelCurrent / 100;
        //aiDisplay.text = state.ToString(); //example code
        //Store the X and Y axis input
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        //Enable or disable particle emission appropriately based on input
        if (ship.getFuel() > 0)
        {
            if (yInput > 0 || xInput != 0)
            {
                animator.SetBool("bThrust", true);
                particles.enableEmission = true;
            }
            else
            {
                animator.SetBool("bThrust", false);
                particles.enableEmission = false;
            }
        }
        else if (ship.getFuel() <= 0)
        {
            particles.enableEmission = false;
        }

        if (Input.GetButton("RRotate"))
        {
            rigidbody.transform.Rotate(new Vector3(0, 0, -5));
            Debug.Log("RRotate triggered");
        }
        else if (Input.GetButton("LRotate"))
        {
            rigidbody.transform.Rotate(new Vector3(0, 0, 5));
            Debug.Log("LRotate triggered");
        }
        else if (Input.GetButton("Thrust"))
        {
            if (ship.getFuel() > 0)
            {
                ship.steerTowards(new Vector2(transform.position.x, transform.position.y) + new Vector2(xInput, yInput),forceX);
                ship.drainFuel(fuelCost);
                Debug.Log("Thrust triggered");
                Debug.Log(ship.getFuel());
            }
        }
        //ship.steerTowards(new Vector2(transform.position.x, transform.position.y) + new Vector2(xInput, yInput));

        else if (Input.GetButtonDown("Shoot"))
        {
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                Debug.Log("Shoot triggered");
                ship.fireWeapon();
                timer = 0;
            }

            //upgradeShip();
        }
    }

    //Take an attack with a force, location of impact, and a damage value
    public void takeAttack(Vector2 force, Vector2 impactLocation, float damage)
    {
        ship.takeImpact(force, impactLocation, damage);
    }

    public Vector2 getVelocity()
    {
        return ship.getVelocity();
    }

    public Rigidbody2D getRigidbody()
    {
        return ship.getRigidbody();
    }

    //do a thing to upgrade the ship
    public void upgradeShip()
    {
        int rng = Random.Range(0, 4);

        if (rng == 0 )
        {
            engineLevel++;
        }
        else if (rng == 1)
        {
            hullLevel++;
        }
        else if (rng == 2)
        {
            thrusterLevel++;
        }
        else if (rng == 3)
        {
            wepLevel++;
        }
        Debug.Log("upgradeShip Called: RNG = " + rng);
        ship.upgradePasser(engineLevel, thrusterLevel, hullLevel, wepLevel, rng, ship);
    }
}