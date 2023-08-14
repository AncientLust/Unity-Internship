using UnityEngine;
using Enums;

public class EnemyDropSystem : MonoBehaviour
{
    private float _firstAidKitSpawnChance = 0.15f;
    private bool _isInitialized;
    private ObjectPool _objectPool;
    private EnemyHealthSystem _enemyHealthSystem;

    public void Init(ObjectPool objectPool, EnemyHealthSystem healthSystem)
    {
        _objectPool = objectPool;
        _enemyHealthSystem = healthSystem;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _enemyHealthSystem.onDie += SpawnRandomPickup;
    }

    private void Unsubscribe()
    {
        _enemyHealthSystem.onDie -= SpawnRandomPickup;
    }

    private void SpawnRandomPickup()
    {
        var chance = Random.value;
        if (chance <= _firstAidKitSpawnChance)
        {
            SpawnFirstAidKit();
        }
    }

    private void SpawnFirstAidKit()
    {
        var obj = _objectPool.Get(EResource.FirstAidKit);
        obj.transform.position = transform.position;
    }
}
