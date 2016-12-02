using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum PlanetType { SUN, HOT, NORM, COLD, GAS };

[System.Serializable]
public struct PlanetInfo
{
    public PlanetInfo(Sprite pSprite, Sprite bSprite, PlanetType typ, float mass, float scle, float threshMin, float threshMax)
    {
        planetSprite = pSprite;
        blurSprite = bSprite;
        type = typ;
        massMultiplier = mass;
        thresholdMin = threshMin;
        thresholdMax = threshMax;
        scale = scle;
    }

    public Sprite blurSprite;
    public Sprite planetSprite;
    public PlanetType type;
    public float massMultiplier;
    public float scale;

    [Range(1, 100)]
    public float thresholdMin;

    [Range(1, 100)]
    public float thresholdMax;

    //Static function for getting a planet by type
    public static PlanetInfo getPlanet(PlanetType searchType, List<PlanetInfo> inList)
    {
        for (int i = 0; i < inList.Count; ++i)
        {
            if (inList[i].type == searchType)
            {
                return inList[i];
            }
        }

        Debug.Log("Attempted to find planet in list, none with searchtype found!");
        return new PlanetInfo(null, null, PlanetType.NORM, 0, 1, 0, 1);
    }
}

public class Planet : MonoBehaviour {
    private List<Rigidbody2D> nearbyBodies;

    PlanetInfo planetInfo;

    float orbitProgress;
    float orbitRate;
    float radius;
    float atmosphereRange;
    float gravityForce;
    float rotator;
    float distanceFromSun;

    Vector2 worldPosition;

    new Collider2D collider;

    //Sets the values for the planet
    public void Setup(PlanetInfo planetInfo, float orbitRate, float distanceFromSun)
    {
        this.planetInfo = planetInfo;
        this.distanceFromSun = distanceFromSun;
        orbitProgress = Random.Range(0, 360);

        collider = gameObject.AddComponent<CircleCollider2D>();
        SpriteRenderer tmpRenderer = gameObject.AddComponent<SpriteRenderer>();
        tmpRenderer.sprite = planetInfo.planetSprite;

        GameObject blurObj = Instantiate(new GameObject("Blur_" + planetInfo.planetSprite.name), this.transform, false) as GameObject;
        SpriteRenderer blurRenderer = blurObj.AddComponent<SpriteRenderer>();
        blurRenderer.sprite = planetInfo.blurSprite;

        float spriteSize = planetInfo.planetSprite.bounds.size.x * planetInfo.scale;

        //TODO: Set scale
        this.transform.localScale = new Vector3(planetInfo.scale, planetInfo.scale, planetInfo.scale);
        radius = spriteSize / 2;
        //collider.radius = radius;

        atmosphereRange = spriteSize * planetInfo.massMultiplier;
        gravityForce = radius / 20 * planetInfo.massMultiplier;

        transform.position = worldPosition;

        rotator = Random.Range(-4, 4);
    }

    //Moves the planet along it's orbit
    public void updateOrbit(float deltaTime)
    {
        //Move orbit if not sun...
        if (planetInfo.type != PlanetType.SUN)
        {
            orbitProgress += orbitRate * deltaTime;
            if (orbitProgress >= 360)
            {
                float overdraft = orbitProgress - 360;
                orbitProgress = overdraft;
            }

            float progressRad = Mathf.Deg2Rad * orbitProgress;

            worldPosition.x = Mathf.Sin(progressRad) * distanceFromSun;
            worldPosition.y = Mathf.Cos(progressRad) * distanceFromSun;

            transform.position = worldPosition;
        }


        transform.Rotate(Vector3.forward, rotator * deltaTime);
    }

    public Vector2 getWorldPosition()
    {
        return worldPosition;
    }

    public float getRadius()
    {
        return radius;
    }

    public float getAtmosphereRange()
    {
        return atmosphereRange;
    }

    public float getGravityForce()
    {
        return gravityForce;
    }

    public PlanetType getPlanetType()
    {
        return planetInfo.type;
    }

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
