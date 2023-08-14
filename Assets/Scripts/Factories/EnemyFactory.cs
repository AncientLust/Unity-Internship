using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public class EnemyFactory : IObjectFactory
{
    private readonly Dictionary<EResource, GameObject> _prefabDict = new();
    private IAudioPlayer _iAudioPlayer;
    private GameSettings _gameSettings;
    private ObjectPool _objectPool;
    private Transform _target;
    public EnemyFactory()
    {
        var prefabs = Resources.LoadAll<GameObject>("Prefabs/Characters");
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

    public void Init
    (
        IAudioPlayer iAudioPlayer,
        GameSettings gameSettings,
        ObjectPool objectPool, 
        Transform target
    )
    {
        _iAudioPlayer = iAudioPlayer;
        _gameSettings = gameSettings;
        _objectPool = objectPool;
        _target = target;
    }

    public GameObject Instantiate(EResource resource)
    {
        if (resource == EResource.EnemyMelee)
        {
            return CreateMeleeEnemy(resource);
        }

        if (resource == EResource.EnemyRange)
        {
            return CreateAndInitRangeEnemy(resource);
        }

        throw new Exception("The resource must be eather EnemyMelee or EnemyRange.");
    }

    private GameObject CreateMeleeEnemy(EResource resource)
    {
        var meleeEnemyObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var meleeEnemy = meleeEnemyObj.GetComponent<MeleeEnemy>();
        meleeEnemy.Init(_iAudioPlayer, _target, _gameSettings, _objectPool);
        return meleeEnemyObj;
    }

    private GameObject CreateAndInitRangeEnemy(EResource resource)
    {
        var rangeEnemyObj = UnityEngine.Object.Instantiate(_prefabDict[resource]);
        var rangeEnemy = rangeEnemyObj.GetComponent<RangeEnemy>();
        rangeEnemy.Init(_iAudioPlayer, _objectPool, _target, _gameSettings);
        return rangeEnemyObj;
    }
}