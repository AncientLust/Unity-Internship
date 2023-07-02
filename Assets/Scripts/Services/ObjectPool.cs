using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public static ObjectPool Instance { get; } = new ObjectPool();
    private readonly Dictionary<PooledObject, Queue<GameObject>> pool;

    private ObjectFactory _objectFactory;

    public ObjectPool()
    {
        pool = new Dictionary<PooledObject, Queue<GameObject>>();
        _objectFactory = new ObjectFactory();
    }

    public GameObject Get(PooledObject objType)
    {
        if (pool.TryGetValue(objType, out var objects) && objects.Count > 0)
        {
            var obj = objects.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = _objectFactory.Instantiate(objType.ToString());
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        if (Enum.TryParse(obj.tag, out PooledObject objType))
        {
            if (!pool.ContainsKey(objType))
            {
                pool[objType] = new Queue<GameObject>();
            }

            obj.SetActive(false);
            pool[objType].Enqueue(obj);
        }
        else
        {
            Debug.LogError("Invalid object type: " + obj.tag);
        }
    }
}
