using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ObjectPool
{
    private Dictionary<EResource, Queue<GameObject>> _resourcePoolMap;
    private Dictionary<EResource, IObjectFactory> _resourceFactoryMap;

    public void Init
    (
        ProjectileFactory projectileFactory, 
        EnemyFactory enemyFactory, 
        EffectFactory effectFactory, 
        GrenadeFactory grenadeFactory,
        PickupFactory pickupFactory
    )
    {
        _resourcePoolMap = new Dictionary<EResource, Queue<GameObject>>();
        _resourceFactoryMap = new Dictionary<EResource, IObjectFactory>()
        {
            { EResource.Bullet, projectileFactory },
            { EResource.Rocket, projectileFactory },
            { EResource.Grenade, grenadeFactory },
            { EResource.EnemyMelee, enemyFactory },
            { EResource.EnemyRange, enemyFactory },
            { EResource.Explosion, effectFactory },
            { EResource.FirstAidKit, pickupFactory }
        };
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
        if (Enum.TryParse(obj.tag, out EResource resource))
        {
            if (!_resourcePoolMap.ContainsKey(resource))
            {
                _resourcePoolMap[resource] = new Queue<GameObject>();
            }

            obj.SetActive(false);
            _resourcePoolMap[resource].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Invalid object type: " + obj.tag);
        }
    }

    public void Reset()
    {
        _resourcePoolMap = new Dictionary<EResource, Queue<GameObject>>();
    }
}
