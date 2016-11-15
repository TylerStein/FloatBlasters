using UnityEngine;
using System.Collections;

//The resource manager is a singleton for holding data that needs to be accessed statically
public class ResourceManager : MonoBehaviour {
    //Singleton instance
    public static ResourceManager instance = null;

    void Awake()
    {
        if(instance == null)
        {
            ResourceManager.instance = this;
            Debug.Log("ResourceManager instance set");
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    //Materials to store
    public Material material_laser;

    //Prefabs to store
    public GameObject prefab_alien_interceptor;

}
