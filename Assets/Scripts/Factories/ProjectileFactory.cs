using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class ProjectileFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict = new();
    private ObjectPool _objectPool;
    private AudioPlayer _audioPlayer;

    public ProjectileFactory()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Projectiles");
        foreach (var prefab in prefabs)
        {
            if (Enum.TryParse(prefab.name, out EResource resource))
            {
                _prefabDict[resource] = prefab;
                continue;
            }

            Debug.LogError($"EResource doesn't have value {prefab.name}.");
        }
    }

    public void Init(ObjectPool objectPool, AudioPlayer audioPlayer)
    {
        _objectPool = objectPool;
        _audioPlayer = audioPlayer;
    }

    public GameObject Instantiate(EResource resource)
    {
        var projectileObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile.Init(_objectPool, _audioPlayer);
        return projectileObj;
    }
}