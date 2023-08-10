using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class GenericFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict = new();

    public GenericFactory()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs");
        foreach (var prefab in prefabs)
        {
            if (Enum.TryParse(prefab.name, out EResource resource))
            {
                _prefabDict[resource] = prefab;
                continue;
            }

            Debug.LogError($"EResource doesn't have value {prefab.name}.");
        }
    }

    public GameObject Instantiate(EResource resource)
    {
        return UnityEngine.Object.Instantiate(_prefabDict[resource]);
    }
}