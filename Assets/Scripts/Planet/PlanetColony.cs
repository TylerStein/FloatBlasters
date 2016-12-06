using UnityEngine;
using System.Collections;

public class PlanetColony : MonoBehaviour {
    public Sprite colonySprite;
    float currHP;
    float maxHP;
    Vector2 worldPosition;
    BoxCollider2D collisionBox;
    bool underAttack;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //spawn colony
    public void spawnColony(float HP, Vector2 pos)
    {
        
        PlanetColony c = new PlanetColony();
        c.currHP = HP;
        c.maxHP = HP;
        c.worldPosition = pos;
        c.collisionBox.size = pos;
        
    }

    //reward from objective
    ShipComponent generateReward( float v, string n)
    {   
        //generate shipComponent
        ShipComponent s = new ShipComponent();
        s.name = n;
        s.value = v;
        return s;
    }

   public void rewardPlayer(int el, int tl, int hl, int wl, int rng, ShipController pship)
    {
        if (rng == 0)
        {
            Debug.Log("rng == 0");
            pship.updateFueltank(el);
        }
        else if (rng == 1)
        {
            Debug.Log("rng == 1");
            pship.updateHull(hl);
        }
        else if (rng == 2)
        {
            Debug.Log("rng == 2");
            pship.updateThruster(tl);
        }
        else if (rng == 3)
        {
            Debug.Log("rng == 3");
            pship.updateWeapon(wl);
        }        
    }

    //gets for the colony
    public float getHP()
    {
        return currHP;
    }

    public void colonyTakeDamage(float dmg)
    {
        currHP = currHP - dmg;

        if (currHP >= 0)
        {
            underAttack = false;
            Destroy(this);
        }

        
    }

    public Vector2 getPosition()
    {
        return worldPosition;
    }

    public Vector2 getHitBox()
    {
        return collisionBox.size;
    }


}
