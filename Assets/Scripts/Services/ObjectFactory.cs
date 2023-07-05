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

    public GameObject InstantiateUndestroyable(EResource resource)
    {
        if (_prefabDict.TryGetValue(resource.ToString(), out var prefab))
        {
            var obj = GameObject.Instantiate(prefab);
            GameObject.DontDestroyOnLoad(obj);
            return obj;
        }
        else
        {
            Debug.LogError($"No prefab found with name {resource}");
            return null;
        }
    }

    //public T Instantiate<T, E>(E item)
    //{
    //    var path = string.Format("{0}/{1}", typeof(E).Name, item.ToString());
    //    var prefab = GameObject.Instantiate(Resources.Load<GameObject>(path));
    //    return prefab.GetComponent<T>();
    //}
}
