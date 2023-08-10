using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class EffectFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict = new();
    private ObjectPool _objectPool;

    public EffectFactory()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Effects");
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

    public void Init(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    public GameObject Instantiate(EResource resource)
    {
        var effectObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var effect = effectObj.GetComponent<Effect>();
        effect.Init(_objectPool);
        return effectObj;
    }
}