using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory
{
    private readonly Dictionary<string, GameObject> _prefabDict;

    public ObjectFactory()
    {
        _prefabDict = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("PrefabsPooled");
        foreach (var prefab in prefabs)
        {
            _prefabDict[prefab.name] = prefab;
        }
    }

    public GameObject Instantiate(string name)
    {
        if (_prefabDict.TryGetValue(name, out var prefab))
        {
            return Object.Instantiate(prefab);
        }
        else
        {
            Debug.LogError($"No prefab found with name {name}");
            return null;
        }
    }
}
