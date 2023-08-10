using Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict;
    private ObjectPool _objectPool;
    private AudioPlayer _audioPlayer;

    public GrenadeFactory()
    {
        _prefabDict = new Dictionary<EResource, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Throwables");
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

    public void Init(ObjectPool objectPool, AudioPlayer audioPlayer)
    {
        _objectPool = objectPool;
        _audioPlayer = audioPlayer;
    }

    public GameObject Instantiate(EResource resource)
    {
        var effectObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var grenade = effectObj.GetComponent<Grenade>();
        grenade.Init(_objectPool, _audioPlayer);
        return effectObj;
    }
}
