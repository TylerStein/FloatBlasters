using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour
{
    Material starfield;
    Transform trns;

    //How much the movement speed should be divided to slow the scrolling
    public float speedDivision = 50000;

    // Use this for initialization
    void Start()
    {
        starfield = GetComponent<MeshRenderer>().material;
        trns = transform;

        resize();
    }

    //Function to resize the quad, useful for screen resizing
    public void resize()
    {
        float height = 2.0f + Mathf.Tan(Camera.main.fieldOfView * Mathf.Deg2Rad) * 80;
        float width = height * Camera.main.aspect;

        transform.localScale = new Vector3(width, height);
    }

    //Take a value to move by (divided by slow amount to figure out final scroll speed)
    public void moveField(Vector2 movement)
    {
        //Close-fast stars
        Vector2 starOffset = starfield.mainTextureOffset;
        starOffset += (movement / speedDivision);

        if (starOffset.x > 1)
        {
            float overdraft = starOffset.x - 1;
            starOffset = new Vector2(overdraft, overdraft);
        }

        starfield.mainTextureOffset = starOffset;
    }
}

