using UnityEngine;
using System.Collections;

public class ShipComponent
{
    public float value { get { return value; } set { this.value = value; } }
    public string name { get { return name; } set { name = value; } }
}

public class Thrusters : ShipComponent
{

    float fwdThrust;
    float maxSpeed;
    float fuelUse;
}

public class Hull : ShipComponent
{

    float reduction;
    float currHP;
    float maxHP;
    bool isDestroyed;

    void takeDamage(float dmg)
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
    float capacity;
    float held;
    bool isEmpty;

    void drainFuel(float amt)
    {
        held -= amt;
        if(held <= 0)
        {
            held = 0;
            isEmpty = true;
        }
    }
}

public class Weapon : ShipComponent
{ 
    float cooldown;
    float damage;
    float dmgForce;
    float range;

    public virtual void fireWeapon() { }
}

public class StaticWeapon : Weapon
{
    Sprite projSprite;
}

public class DeployableWeapon : Weapon
{
    Transform targetTransform;
    GameObject deployable;
}

public class SwivelWeapon : Weapon
{
    Sprite projSprite;
    Vector2 targetPos;
}