using UnityEngine;
using System.Collections;

//Possible types of alien ships
public enum AlienShipType { INTERCEPTOR, FIGHTER, DRONE, BOMBER };
public enum WeaponType { STOCK, DRONE, SWIVEL };
public class ShipController : MonoBehaviour
{
    //Physics Vars
    float mass;
    public Material laserMaterial;
    //Components
    new Transform transform;
    Sprite shipSprite;
    new Collider2D collider2D;
    new Rigidbody2D rigidbody;
    WeaponType wType;

    //Ship Parts
    Thrusters thrusters;
    Hull hull;
    Fueltank fuel;
    Weapon weapon;
    StaticWeapon stockWep;
    // Use this for initialization
    void Awake () {
        transform = gameObject.transform;
        collider2D = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void steerTowards(Vector2 dir, float thrust)
    {
        //* thrusterForce
        rigidbody.AddForce(transform.up * Time.deltaTime * thrust, ForceMode2D.Force);

        //float angleTo = Vector2.Angle(transform.up, dir);
        //rigidbody.AddTorque(thrust * angleTo * Time.deltaTime, ForceMode2D.Force);
    }

    public void fireWeapon()
    {
        if (wType == WeaponType.STOCK)
        {
            stockWep.fireStaticWeapon(transform.position, transform.up);
        }
        else if (wType == WeaponType.SWIVEL)
        {

        }
        else if (wType == WeaponType.DRONE)
        {

        }
    }
    //thruster methods
    public float getFuelCost()
    {
        return thrusters.fuelUse;
    }
    //hull methods
    public float getCurrentHealth()
    {
        return hull.currHP;
    }
    public bool getisDestroyed()
    {
        return hull.isDestroyed;
    }
    public void takeImpact(Vector2 force, Vector2 point, float dmg)
    {
        rigidbody.AddForceAtPosition(force * Time.deltaTime, point, ForceMode2D.Impulse);
        //account for armor
        float dmgmod = hull.reduction * dmg;
        dmg -= dmgmod;
        hull.takeDamage(dmg);
    }

    //fueltank methods
    public void drainFuel(float amt)
    {
        fuel.drainFuel(amt);
    }

    public float getFuel()
    {
        return fuel.held;
    }
    public bool getIsEmpty()
    {
        return fuel.isEmpty;
    }
    //this.method
    public Rigidbody2D getRigidbody()
    {
        return rigidbody;
    }

    public Vector2 getVelocity()
    {
        return rigidbody.velocity;
    }

    //init
    public void defaultFitting(float cap, float hld, float reduction, float mHP, float cHP, float cost, float fThrust, float mSpeed)
    {
        fuel = new Fueltank(cap,hld,false);
        Debug.Log("Fueltank cap = " + cap + " heldfuel = " + hld);

        hull = new Hull(reduction,cHP,mHP,false);
        Debug.Log("Hulls reduction = " + reduction + " maxHealth = " + mHP);

        thrusters = new Thrusters(fThrust, mSpeed, cost);
        Debug.Log("Thrusters propulsion = " + fThrust + " maxSpeed = " + mSpeed + " and the cost of fuel is " + cost);
    }

    public void weaponFitting(float cd, float dmg, float dmgF, float rang)
    {
        //weapon = new Weapon(cd,dmg,dmgF,rang);
        stockWep = new StaticWeapon(cd, dmg, dmgF, rang);
        stockWep.laserMat = laserMaterial;
        Debug.Log("Weapons Online.../ cooldown = " + cd + " damage = " + dmg + ", the knockback force is " + dmgF + " and the targeting systems range is" + rang);
        wType = WeaponType.STOCK;
    }
    //creation methods
    public void updateFueltank(int upgradelevel)
    {
        fuel.held = fuel.held + ((upgradelevel / 5) * fuel.held); //20% more fuel per upgrade level
        fuel.capacity = fuel.capacity + ((upgradelevel / 5) * fuel.capacity); //20% more fuel capacity per upgrade level
    }

    public void updateHull(int upgradelevel)
    {
        hull.reduction = hull.reduction + ((upgradelevel / 5) * hull.reduction); //20% more damage resist per upgrade level
        hull.currHP = hull.currHP + ((upgradelevel / 5) * hull.currHP); //20% more health per upgrade level
        hull.maxHP = hull.maxHP + ((upgradelevel / 5) * hull.maxHP); //20% more health per upgrade level
    }

    public void thrusterHull(int upgradelevel)
    {
        ////////////thrusters.fuelUse = thrusters.fuelUse + ((upgradelevel / 5) * thrusters.fuelUse); //20% less fule guzzled per upgrade level
        thrusters.fwdThrust = thrusters.fwdThrust + ((upgradelevel / 5) * thrusters.fwdThrust); //20% more thrust per upgrade level
        thrusters.maxSpeed = thrusters.maxSpeed + ((upgradelevel / 5) * thrusters.maxSpeed); //20% speed cap per upgrade level
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