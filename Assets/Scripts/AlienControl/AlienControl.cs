using UnityEngine;
using System.Collections;

public class AlienControl : MonoBehaviour {

    Rigidbody2D rigidbody;

    public Vector2[] weaponPositions;

    public Transform player;
    public AlienShip ship;

    public float maxTurnAngle = 20;
    public float forwardThrusterPower = 1;
    public float sideThrusterPower = 0.25f;
    public float stopDist = 2;

    public Vector2 leftThrusterPos;
    public Vector2 rightThrusterPos;

    private Transform transform;

    // Use this for initialization
    void Start () {
        Init();
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

        //If no ship, default to interceptor
        if (ship == null) { ship = new Interceptor(this); }

        ship.setWeaponSources(weaponPositions);
    }
	// Update is called once per frame
	void Update () {
       
    }

    public void setShipType(ShipType type)
    {
        switch (type)
        {
            case ShipType.Interceptor:
                ship = new Interceptor(this);
                break;
            default:
                ship = new Interceptor(this);
                break;
        }
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
        /*Fire weapon*/
        ship.FireWeapon(target);
    }

    public Rigidbody2D getRigidbody()
    {
        return rigidbody;
    }

    public Vector2 getPosition()
    {
        return transform.position;
    }

    public Vector2 getUp()
    {
        return transform.up;
    }

    public Vector2 getVelocity()
    {
        return rigidbody.velocity;
    }

    public void setPosition(Vector2 pos)
    {
        transform.position = pos;
    }
}

public enum ShipType { Interceptor }

public class AlienShip
{
    public Vector2[] weaponSources;
    public Sprite shipSprite;
    public AlienControl control;

    public AlienShip(Vector2 src, AlienControl ctrl) { addWeaponSource(src); control = ctrl; }
    public AlienShip(Vector2[] src, AlienControl ctrl) {setWeaponSources(src); control = ctrl; }
    public AlienShip(AlienControl ctrl) { control = ctrl; }
    public virtual void FireWeapon(Vector2 target) { }


    public void setWeaponSources(Vector2[] src)
    {
        weaponSources = new Vector2[src.Length];
        src.CopyTo(weaponSources, 0);
    }

    public void addWeaponSource(Vector2 src)
    {
        //Initialize weapon sources array if not already
        if(weaponSources == null) { weaponSources = new Vector2[1]; }

        //Add the weapon source to the list
        Vector2[] storage = weaponSources;
        weaponSources = new Vector2[storage.Length + 1];
        for(int i = 0; i < storage.Length; ++i)
        {
            weaponSources[i] = storage[i];
        }
        weaponSources[weaponSources.Length - 1] = src;
    }
}

public class Interceptor : AlienShip
{
    const float weaponForce = 500;
    const float weaponDamage = 15;
    const float weaponRange = 25;

    Material laserMat;

    public Interceptor(AlienControl ctrl) : base(ctrl)
    {
        laserMat = ResourceManager.instance.material_laser;
        
    }

    public override void FireWeapon(Vector2 target)
    {
        Vector2 shipPos = new Vector2(control.getPosition().x, control.getPosition().y);

        //Fire from each weapon source
        for (int i = 0; i < weaponSources.Length; ++i)
        {
            Vector2 weaponSource = (weaponSources[i] + shipPos);

            //Figure out the weapon's firing direction
            Vector2 dir = control.getUp();

            //Set up the lazer effect
            LineRenderer tmpRender = setupLaserEffect(weaponSource, dir * weaponRange);

            //Do a 2d raycast to check if we hit the target with this lazer
            RaycastHit2D hit = Physics2D.Raycast(weaponSource, dir, 25, 0, -1, 1);
            if ((hit.collider != null))
            {
                tmpRender.SetPosition(1, new Vector3(hit.point.x, hit.point.y, 0));
                if ((hit.collider.tag == "Player"))
                {
                    Debug.Log("Hit player");
                    //Hit the player, apply impact force and damage
                    PlayerControl plyr = hit.collider.gameObject.GetComponent<PlayerControl>();
                    Vector2 impactPoint = hit.point;
                    Vector2 force = (impactPoint - weaponSource).normalized;
                    force *= weaponForce;
                    plyr.takeAttack(force, impactPoint, weaponDamage);
                }
            }
        }
    }

    LineRenderer setupLaserEffect(Vector2 source, Vector2 target)
    {
        //Create a lazer object and contained line renderer
        GameObject rndObj = new GameObject("Laser");
        rndObj.transform.position = new Vector3(source.x, source.y, 0);
        LineRenderer rnd = rndObj.AddComponent<LineRenderer>();

        rnd.useWorldSpace = true;

        //Lazer points
        Vector3[] lzrPoints = new[] {
            new Vector3(source.x, source.y, 0),
            new Vector3(target.x, target.y, 0)
        };

        //Set up the lazer
        rnd.SetPositions(lzrPoints);
        rnd.SetWidth(0.25f, 0.25f);

        //Get the lazer material
        rnd.material = laserMat;

        rndObj.AddComponent<SelfDestruct>();

        return rnd;
    }
}