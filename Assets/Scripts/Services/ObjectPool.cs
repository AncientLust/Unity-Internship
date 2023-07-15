using System;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ObjectPool
{
    public static ObjectPool Instance { get; } = new ObjectPool();
    private readonly Dictionary<EResource, Queue<GameObject>> pool;

    private ObjectFactory _objectFactory;

    public ObjectPool()
    {
        pool = new Dictionary<EResource, Queue<GameObject>>();
        _objectFactory = new ObjectFactory();
    }

    public GameObject Get(EResource resource)
    {
        if (pool.TryGetValue(resource, out var objects) && objects.Count > 0)
        {
            var obj = objects.Dequeue();           
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = _objectFactory.Instantiate(resource);
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        if (Enum.TryParse(obj.tag, out EResource resource))
        {
            if (!pool.ContainsKey(resource))
            {
                pool[resource] = new Queue<GameObject>();
            }

            obj.SetActive(false);               
            pool[resource].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Invalid object type: " + obj.tag);
        }
    }
}
