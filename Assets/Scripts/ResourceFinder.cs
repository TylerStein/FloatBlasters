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

    private Dictionary<string, Object> prefabCache;
    private Dictionary<string, Material> materialCache;

    //Instance the dictionary and fill the prefab cache
    private void Setup()
    {
        prefabCache = new Dictionary<string, Object>();

        Object[] baseRes = Resources.LoadAll("Prefabs");
        foreach(Object o in baseRes)
        {
            string name = o.name;
            Debug.Log("@ResourceFinder found prefab \"" + name + "\" in Prefabs");
            prefabCache.Add(name, o);
        }

        Debug.Log("@ResourceFinder Found " + prefabCache.Count + " prefabs");

        materialCache = new Dictionary<string, Material>();

        Material[] baseMat = Resources.LoadAll<Material>("Materials") as Material[];
        foreach(Material m in baseMat)
        {
            string name = m.name;
            Debug.Log("@ResourceFinder found Material \"" + name + "\" in prefab");
            materialCache.Add(name, m);
        }

        Debug.Log("@ResourceFinder Found " + materialCache.Count + " materials");
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

    public Material GetMaterial(string name)
    {
        //Use dictionary enumerator search, it's faster than a for loop
        var enumerator = materialCache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var pair = enumerator.Current;
            if (name == pair.Key)
            {
                return pair.Value as Material;
            }
        }
        Debug.Log("Material\"" + name + "\" not found!");
        return null;
    }
}
