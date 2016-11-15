using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : MonoBehaviour{

    public Sprite planetSprite;
    public Sprite blurSprite;
    public float gravityForce = 2;

    private List<Rigidbody2D> nearbyBodies;

    float planetScale = 1;
    float atmosphereRange;

    PlanetType planetType;

    Vector2 worldPosition;
    float distanceFromSun;
    float orbitRate;
    float orbitProgress;
    float rotator;
    float mass;
    float radius;

    CircleCollider2D collider;

    public void Setup(float distFromSun, float orbitRate, float mass, PlanetType type, Sprite sprite, Sprite bSprite)
    {
        Debug.Log("Setting up planet with dist " + distFromSun);
        distanceFromSun = distFromSun;
        planetType = type;
        planetSprite = sprite;
        blurSprite = bSprite;

        worldPosition.x = distanceFromSun;
        worldPosition.y = 0;

        this.orbitRate = orbitRate;
        this.mass = mass;

        orbitProgress = Random.Range(0, 360);

        collider = gameObject.AddComponent<CircleCollider2D>();
        SpriteRenderer tmpRenderer = gameObject.AddComponent<SpriteRenderer>();
        tmpRenderer.sprite = sprite;

        GameObject blurObj = Instantiate(new GameObject("Blur_" + sprite.name), this.transform, false) as GameObject;
        SpriteRenderer blurRenderer = blurObj.AddComponent<SpriteRenderer>();
        blurRenderer.sprite = blurSprite;

       

        float spriteSize = sprite.bounds.size.x * planetScale;

        //TODO: Set scale
        this.transform.localScale = new Vector3(planetScale, planetScale, planetScale);
        radius = spriteSize / 2;
        collider.radius = radius;

        atmosphereRange = spriteSize * mass;
        gravityForce = radius / 20 * mass;

        transform.position = worldPosition;

        rotator = Random.Range(-4, 4);
    }


    public void updateOrbit(float deltaTime)
    {
        //Move orbit if not sun...
        if (planetType != PlanetType.SUN)
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
        return planetType;
    }
}