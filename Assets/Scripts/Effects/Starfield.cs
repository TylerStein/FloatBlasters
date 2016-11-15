using UnityEngine;
using System.Collections;

public class Starfield : MonoBehaviour {

    Material starfield;
    Transform trns;

    public float speedDivision = 50000;

    // Use this for initialization
    void Start () {
        starfield = GetComponent<MeshRenderer>().material;
        trns = transform;

        Vector2 screenBounds = new Vector2(Screen.width, Screen.height);
        float size = Camera.main.orthographicSize / 10;

        Vector3 fillSize = new Vector3(screenBounds.x / size, screenBounds.y / size, 1);

        transform.localScale = fillSize;
    }

    public void resize()
    {
        Vector2 screenBounds = new Vector2(Screen.width, Screen.height);
        float size = Camera.main.orthographicSize / 100;

        Vector3 fillSize = new Vector3(screenBounds.x / size, screenBounds.y / size, 1);

        transform.localScale = fillSize;
    }

    public void moveField(Vector2 movement)
    {
        //Close-fast stars
        Vector2 starOffset = starfield.mainTextureOffset;
        starOffset += (movement / speedDivision);

        if(starOffset.x > 1)
        {
            float overdraft = starOffset.x - 1;
            starOffset = new Vector2(overdraft, overdraft);
        }

        starfield.mainTextureOffset = starOffset;
    }

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
