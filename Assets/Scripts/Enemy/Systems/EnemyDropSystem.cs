using UnityEngine;
using Enums;

public class EnemyDropSystem : MonoBehaviour
{
    private float _firstAidKitSpawnChance = 0.15f;
    private float _slowMotionSpawnChance = 0.10f;
    private float _speedUpSpawnChance = 0.10f;
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
        if (Random.value <= _speedUpSpawnChance)
        {
            SpawnSpeedUp();
        }
        else if (Random.value <= _firstAidKitSpawnChance)
        {
            SpawnFirstAidKit();
        }
        else if (Random.value <= _slowMotionSpawnChance)
        {
            SpawnSlowmotion();
        }
    }

    private void SpawnFirstAidKit()
    {
        var obj = _objectPool.Get(EResource.FirstAidKit);
        obj.transform.position = transform.position;
    }

    private void SpawnSlowmotion()
    {
        var obj = _objectPool.Get(EResource.SlowMotion);
        obj.transform.position = transform.position;
    }
    
    private void SpawnSpeedUp()
    {
        var obj = _objectPool.Get(EResource.SpeedUp);
        obj.transform.position = transform.position;
    }
}
