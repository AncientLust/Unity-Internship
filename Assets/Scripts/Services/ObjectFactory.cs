using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ObjectFactory
{
    private readonly Dictionary<string, GameObject> _prefabDict;

    public ObjectFactory()
    {
        _prefabDict = new Dictionary<string, GameObject>();

        var prefabs = Resources.LoadAll<GameObject>("");

        foreach (var prefab in prefabs)
        {
            _prefabDict[prefab.name] = prefab;
        }
    }

    public GameObject Instantiate(EResource resource)
    {
        if (_prefabDict.TryGetValue(resource.ToString(), out var prefab))
        {
            return Object.Instantiate(prefab);
        }
        else
        {
            Debug.LogError($"No prefab found with name {resource}");
            return null;
        }
    }
}
