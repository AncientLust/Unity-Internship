using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    public static ObjectPool<T> SharedInstance;

    [SerializeField] protected List<GameObject> _pooledObjects;
    [SerializeField] protected GameObject _objectToPool;
    [SerializeField] protected int _amountToPool;
    
    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < _amountToPool; i++)
        {
            tmp = Instantiate(_objectToPool, transform);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _amountToPool; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }
        return null;
    }
}
