using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControl : MonoBehaviour {

    Rigidbody2D rigidbody;

    public float forwardThrusterPower = 1;
    public float sideThrusterPower = 0.25f;

    public Animator animator;
    public ParticleSystem particles;

    public Vector2 leftThrusterPos;
    public Vector2 rightThrusterPos;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        Sprite playerSprite = GetComponent<SpriteRenderer>().sprite;
        Vector2 shipBounds = new Vector2(playerSprite.bounds.extents.x, playerSprite.bounds.extents.y);
        animator = GetComponent<Animator>();

        particles = GetComponentInChildren<ParticleSystem>();

        gameObject.layer = ResourceManager.instance.layer_players;

        leftThrusterPos = new Vector2(shipBounds.x / 2, 0);
        rightThrusterPos = new Vector2(shipBounds.x, 0);
    }
	
	// Update is called once per frame
	void Update () {
        float yInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");

        if(yInput > 0 || xInput != 0) {
            animator.SetBool("bThrust", true);
            particles.enableEmission = true;
        }
        else {
            animator.SetBool("bThrust", false);
            particles.enableEmission = false;
        }

        rigidbody.AddRelativeForce(Vector2.up * yInput * forwardThrusterPower);
        rigidbody.AddTorque(-xInput * sideThrusterPower);

    }

    public void takeAttack(Vector2 force, Vector2 impactLocation, float damage)
    {
        rigidbody.AddForceAtPosition(force / Time.deltaTime, impactLocation, ForceMode2D.Impulse);
        Debug.Log("Got hit with " + damage + " damage!");
    }

    public Vector2 getVelocity()
    {
        return rigidbody.velocity;
    }

    public Rigidbody2D getRigidbody()
    {
        return rigidbody;
    }
}
