using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ProjectileFactory : IObjectFactory
{
    private readonly Dictionary<string, GameObject> _prefabDict;
    private ObjectPool _objectPool;

    public ProjectileFactory()
    {
        _prefabDict = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("Projectiles");
        foreach (var prefab in prefabs)
        {
            _prefabDict[prefab.name] = prefab;
        }
    }

    public void Init(ObjectPool objectPool)
    {
        _objectPool = objectPool;
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