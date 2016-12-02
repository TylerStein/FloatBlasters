using UnityEngine;
using System.Collections;

enum ShipType { INTERCEPTOR, COMMANDER, BOMBER }
public class AlienShip : MonoBehaviour {

    ShipType shipType;
    ShipController ship;

    new Rigidbody2D rigidbody;

    public Vector2[] weaponPositions;

    public Transform player;

    public Animator animator;

    public float maxTurnAngle = 20;
    public float forwardThrusterPower = 1;
    public float sideThrusterPower = 0.25f;
    public float stopDist = 2;

    public Vector2 leftThrusterPos;
    public Vector2 rightThrusterPos;

    new private Transform transform;


    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        SpriteRenderer sprRend = gameObject.GetComponent<SpriteRenderer>();
        Sprite tmp = sprRend.sprite;

        Vector2 shipBounds = new Vector2(tmp.bounds.extents.x, tmp.bounds.extents.y);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        leftThrusterPos = new Vector2(shipBounds.x / 2, 0);
        rightThrusterPos = new Vector2(shipBounds.x, 0);

        transform = gameObject.transform;

        animator = GetComponent<Animator>();
        animator.SetBool("bThrust", true);
    }

    public void seekTarget(Vector2 target)
    {
        //Get player diff vector
        Vector2 playerDiff = target - new Vector2(transform.position.x, transform.position.y);

        //Target player
        Vector2 targetVector = playerDiff.normalized;

        //Get the normalized forward vector
        Vector3 fwdNormal = transform.up.normalized;
        Vector2 forwardVector = new Vector2(fwdNormal.x, fwdNormal.y);

        //Figure out the sign of the angle
        Vector3 cross = Vector3.Cross(targetVector, forwardVector);

        //Get the sign
        float sign = Mathf.Sign(cross.z);

        //Get the angle
        float angle = Vector2.Angle(targetVector, forwardVector);
        angle *= sign;

        Mathf.Clamp(angle, -maxTurnAngle, maxTurnAngle);

        //Attempt to angle towards the desired vecoty
        rigidbody.AddTorque(-angle * sideThrusterPower);
        rigidbody.AddForce(fwdNormal * forwardThrusterPower);
    }

    public void fireAt(Vector2 target)
    {

    }

    public Vector2 getPosition()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }
}
