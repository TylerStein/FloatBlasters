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
        float h = Mathf.Tan(Camera.main.orthographicSize * Mathf.Deg2Rad * 0.5f) * 30f;
        transform.localScale = new Vector3(h * Camera.main.aspect, 1, h);
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

    //Set the scale of the starfield
    public void scaleField(float scale)
    {
        Vector2 newScale = Vector2.one * scale;
        starfield.mainTextureScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

