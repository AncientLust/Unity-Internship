using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;

public class ObjectPool
{
    private Dictionary<EResource, Queue<GameObject>> _resourcePoolMap;
    private Dictionary<EResource, IObjectFactory> _resourceFactoryMap;

    public void Init(FactoryGroup factoryGroup)
    {
        CreateResourceFactoryMap(factoryGroup);
        CreateResourcePoolMap();
    }

    private void CreateResourceFactoryMap(FactoryGroup factoryGroup)
    {
        _resourceFactoryMap = new Dictionary<EResource, IObjectFactory>(){
            { EResource.Bullet, factoryGroup.projectileFactory },
            { EResource.Rocket, factoryGroup.projectileFactory },
            { EResource.Grenade, factoryGroup.grenadeFactory },
            { EResource.EnemyMelee, factoryGroup.enemyFactory },
            { EResource.EnemyRange, factoryGroup.enemyFactory },
            { EResource.Explosion, factoryGroup.effectFactory },
            { EResource.FirstAidKit, factoryGroup.pickupFactory },
            { EResource.SlowMotion, factoryGroup.pickupFactory },
            { EResource.SpeedUp, factoryGroup.pickupFactory }
        };
    }

    private void CreateResourcePoolMap()
    {
        _resourcePoolMap = new Dictionary<EResource, Queue<GameObject>>();
        foreach (KeyValuePair<EResource, IObjectFactory> entry in _resourceFactoryMap)
        {
            _resourcePoolMap[entry.Key] = new Queue<GameObject>();
        }
    }

    public GameObject Get(EResource resource)
    {
        if (_resourcePoolMap.TryGetValue(resource, out var objects) && objects.Count > 0)
        {
            var obj = objects.Dequeue();           
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var factory = _resourceFactoryMap[resource];
            var obj = factory.Instantiate(resource);
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        var pooledResource = obj.GetComponent<PooledResource>();
        if (pooledResource != null)
        {
            var resource = pooledResource.type;
            obj.SetActive(false);
            _resourcePoolMap[resource].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Returned game object doesn't have PooledResource component attached");
        }
    }

    public void Reset()
    {
        foreach (var keyValuePair in _resourcePoolMap)
        {
            keyValuePair.Value.Clear();
        }
    }
}
