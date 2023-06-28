using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public static ObjectPool Instance { get; } = new ObjectPool();
    private readonly Dictionary<string, Queue<GameObject>> pool;

    private ObjectFactory _objectFactory;

    public ObjectPool()
    {
        pool = new Dictionary<string, Queue<GameObject>>();
        _objectFactory = new ObjectFactory();
    }

    public GameObject Get(string name)
    {
        if (pool.TryGetValue(name, out var objects) && objects.Count > 0)
        {
            var obj = objects.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            var obj = _objectFactory.Instantiate(name);
            return obj;
        }
    }

    public void Return(GameObject obj)
    {
        // Use enums instead of strings
        string name = obj.name.Replace("(Clone)", "").Trim();

        if (!pool.ContainsKey(name))
        {
            pool[name] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        pool[name].Enqueue(obj);
    }
}
