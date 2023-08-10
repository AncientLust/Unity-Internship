using System.Collections.Generic;
using UnityEngine;
using Enums;

public class ProjectileFactory : IObjectFactory
{
    private readonly Dictionary<string, GameObject> _prefabDict;
    private ObjectPool _objectPool;
    private AudioPlayer _audioPlayer;

    public ProjectileFactory()
    {
        _prefabDict = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Projectiles");
        foreach (var prefab in prefabs)
        {
            _prefabDict[prefab.name] = prefab;
        }
    }

    public void Init(ObjectPool objectPool, AudioPlayer audioPlayer)
    {
        _objectPool = objectPool;
        _audioPlayer = audioPlayer;
    }

    public GameObject Instantiate(EResource resource)
    {
        if (_prefabDict.TryGetValue(resource.ToString(), out var prefab))
        {
            var projectileObj = Object.Instantiate(prefab);
            var projectile = projectileObj.GetComponent<Projectile>();
            projectile.Init(_objectPool, _audioPlayer);
            return projectileObj;
        }
        else
        {
            Debug.LogError($"No prefab found with name {resource}");
            return null;
        }
    }
}