using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class EffectFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict;
    private ObjectPool _objectPool;

    public EffectFactory()
    {
        _prefabDict = new Dictionary<EResource, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Effects");
        foreach (var prefab in prefabs)
        {
            if (Enum.TryParse(prefab.name, out EResource resource))
            {
                _prefabDict[resource] = prefab;
            }
            else
            {
                Debug.LogError($"EResource doesn't have {prefab.name} element.");
            }
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