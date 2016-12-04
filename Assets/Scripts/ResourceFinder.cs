using UnityEngine;
using System.Collections.Generic;

public class ResourceFinder
{
    private ResourceFinder() { Setup(); }

    private static ResourceFinder _instance;
    public static ResourceFinder Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ResourceFinder();
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private Dictionary<string, object> prefabCache;

    //Instance the dictionary and fill the prefab cache
    private void Setup()
    {
        prefabCache = new Dictionary<string, object>();

        Object[] baseRes = Resources.LoadAll("Prefabs");
        foreach(Object o in baseRes)
        {
            string name = o.name;
            Debug.Log("@ResourceFinder found prefab \"" + name + "\" in Prefabs");
            prefabCache.Add(name, o);
        }

        Debug.Log("@ResourceFinder Found " + prefabCache.Count + " prefabs");
    }


    //Search for the desired prefab by name
    public GameObject GetPrefab(string name)
    {
        //Use dictionary enumerator search, it's faster than a for loop
        var enumerator = prefabCache.GetEnumerator();
        while(enumerator.MoveNext())
        {
            var pair = enumerator.Current;
            if (name == pair.Key)
            {
                return pair.Value as GameObject;
            }
        }
        Debug.Log("Prefab\"" + name + "\" not found!");
        return null;
    }
}
