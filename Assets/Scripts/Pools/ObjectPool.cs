using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] protected GameObject _objectToPool;
    [SerializeField] protected int _amountToPool;

    protected List<GameObject> _pooledObjects;
    public static ObjectPool<T> SharedInstance;
    
    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        InitiatePool();
    }

    protected void InitiatePool()
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
