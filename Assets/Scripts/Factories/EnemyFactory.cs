using System.Collections.Generic;
using UnityEngine;
using Enums;

public class EnemyFactory : IObjectFactory
{
    private readonly Dictionary<string, GameObject> _prefabDict;
    private ObjectPool _objectPool;
    private Transform _target;
    public EnemyFactory()
    {
        _prefabDict = new Dictionary<string, GameObject>();
        var prefabs = Resources.LoadAll<GameObject>("Characters");
        foreach (var prefab in prefabs)
        {
            _prefabDict[prefab.name] = prefab;
        }
    }

    public void Init
    (
        ObjectPool objectPool, 
        Transform target
    )
    {
        _objectPool = objectPool;
        _target = target;
    }

    public GameObject Instantiate(EResource resource)
    {
        if (_prefabDict.TryGetValue(resource.ToString(), out var prefab))
        {
            if (prefab.CompareTag(EResource.EnemyMelee.ToString()))
            {
                return CreateMeleeEnemy(prefab);
            }

            if (prefab.CompareTag(EResource.EnemyRange.ToString()))
            {
                return CreateAndInitRangeEnemy(prefab);
            }

            throw new System.Exception("");
        }
        else
        {
            Debug.LogError($"No prefab found with name {resource}");
            return null;
        }
    }

    private GameObject CreateMeleeEnemy(GameObject prefab)
    {
        var meleeEnemyObj = Object.Instantiate(prefab);
        var meleeEnemy = meleeEnemyObj.GetComponent<MeleeEnemy>();
        meleeEnemy.Init(_target);
        return meleeEnemyObj;
    }

    private GameObject CreateAndInitRangeEnemy(GameObject prefab)
    {
        var rangeEnemyObj = Object.Instantiate(prefab);
        var rangeEnemy = rangeEnemyObj.GetComponent<RangeEnemy>();
        rangeEnemy.Init(_objectPool, _target);
        return rangeEnemyObj;
    }
}