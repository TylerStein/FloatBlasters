using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

    public Starfield[] stars;
    public Vector2 moveRate;

    public Transform earthSprite;
    public float rotRate;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        foreach (Starfield s in stars)
        {
            s.moveField(moveRate);
        }

        earthSprite.Rotate(Vector3.forward, Time.deltaTime * rotRate);
    }

    public void StartMultiplayer()
    {
        PlayerManager.Instance.isMultiplayer = true;
        SceneManager.LoadScene(1);
    }

    public void StartSingleplayer()
    {
        PlayerManager.Instance.isMultiplayer = false;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
