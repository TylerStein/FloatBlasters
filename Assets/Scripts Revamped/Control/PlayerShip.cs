using UnityEngine;
using System.Collections;

public class PlayerShip : MonoBehaviour {

    int playerSlot;
    ShipController ship;

    //Component references
    public Animator animator;
    public ParticleSystem particles;

    // Use this for initialization
    void Start () {
        ship = gameObject.AddComponent<ShipController>();
        animator = GetComponent<Animator>();
        particles = GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        //Store the X and Y axis input
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        //Enable or disable particle emission appropriately based on input
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

        ship.steerTowards(new Vector2(transform.position.x, transform.position.y) + new Vector2(xInput, yInput));
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
}
