using UnityEngine;
using System.Collections;

public class ShipComponent
{
    public float value { get { return value; } set { this.value = value; } }
    public string name { get { return name; } set { name = value; } }
}

public class Thrusters : ShipComponent
{

    public float fwdThrust;
    public float maxSpeed;
    public float fuelUse;

    public Thrusters(float fwdT, float mSpd, float fuel)
    {
        fwdThrust = fwdT;
        maxSpeed = mSpd;
        fuelUse = fuel;
    }
}

public class Hull : ShipComponent
{
    public float reduction;
    public float currHP;
    public float maxHP;
    public bool isDestroyed;

    public Hull(float DR, float cHP, float mHP, bool destroyed)
    {
        reduction = DR;
        currHP = cHP;
        maxHP = mHP;
        isDestroyed = destroyed;
    }
    public void takeDamage(float dmg)
    {
        currHP -= dmg;
        if (currHP <= 0)
        {
            currHP = 0;
            isDestroyed = true;
        }
    }
}

public class Fueltank : ShipComponent
{
    public float capacity;
    public float held;
    public bool isEmpty;

    public Fueltank(float cap, float hld, bool empty)
    {
        capacity = cap;
        held = hld;
        isEmpty = empty;
    }
    public void drainFuel(float amt)
    {
        held -= amt;
        if (held <= 0)
        {
            held = 0;
            isEmpty = true;
        }
    }
}

public class Weapon : ShipComponent
{ 
    
    protected float cooldown;
    protected float damage;
    protected float dmgForce;
    protected float range;
    public Weapon(float cd, float dmg, float dmgF, float rang)
    {
        cooldown = cd;
        damage = dmg;
        dmgForce = dmgF;
        range = rang;
    }
    public virtual void fireWeapon()
    {

    }
}

public class StaticWeapon : Weapon
{
    public Material laserMat;
    //ResourceFinder

    public StaticWeapon(float cd, float dmg, float dmgF, float rang) : base(cd, dmg, dmgF, rang)
    {
        laserMat = ResourceFinder.Instance.GetMaterial("Laser");
    }
    public void fireStaticWeapon(Vector2 playerpos, Vector2 forward)
    {
        Vector3[] points = new Vector3[2];
        points[0] = playerpos;
        points[1] = playerpos + forward * range;
        RaycastHit2D laser;
        GameObject laserObject = new GameObject();
        LineRenderer sight = laserObject.AddComponent<LineRenderer>();


        laser = Physics2D.Raycast(playerpos,forward,range);
        
        if (laser.collider != null)
        {
            points[1] = laser.point;
            if (laser.collider.tag == "Alien")
            {
               AlienShip alienShip = laser.collider.gameObject.GetComponent<AlienShip>();
                alienShip.ship.takeImpact(forward.normalized * dmgForce,laser.point, damage);
            }
        }

        sight.SetPositions(points);
        sight.material = laserMat;
        sight.SetWidth(0.25f, 0.25f);
        sight.useWorldSpace = true;

        GameObject.Destroy(laserObject, 0.1f);
    }
}

public class DeployableWeapon : Weapon
{
    Transform targetTransform;
    GameObject deployable;

    public DeployableWeapon(float cd, float dmg, float dmgF, float rang) : base(cd, dmg, dmgF, rang)
    {

    }
    public void launchDrones()
    {

    }
}

public class SwivelWeapon : Weapon
{
    new Rigidbody2D sTurret;
    Sprite projSprite;
    Vector2 targetPos;

    public SwivelWeapon(float cd, float dmg, float dmgF, float rang) : base(cd, dmg, dmgF, rang)
    {

    }

    public void swivelLeft(float deg)
    {
        sTurret.rotation += deg;
    }

    public void swivelRight(float deg)
    {
        sTurret.rotation -= deg;
    }
}