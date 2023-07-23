using System.Collections;
using UnityEngine;
using Enums;

public class EnemySpawner : MonoBehaviour
{
    protected Transform _target;
    protected IExperienceTaker _experienceTaker;
    protected ObjectPool _objectPool;

    protected float _minRadius = 10f;
    protected float _maxRadius = 20f;
    protected float _spawnRaduis = 360f;
    protected float _minEnemySpawnTime = 1;
    protected float _maxEnemySpawnTime = 2;
    protected int _minEnemiesToSpawn = 1;
    protected int _maxEnemiesToSpawn = 3;
    protected float _meleeEnemySpawnChance = .75f;
    protected Coroutine _spawnCoroutine;

    public void Init(Transform playerTransform, IExperienceTaker experienceTaker, ObjectPool objectPool)
    {
        _target = playerTransform;
        _experienceTaker = experienceTaker;
        _objectPool = objectPool;
    }

    protected void OnDestroy()
    {
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
    }

    public void StartSpawn()
    {
        _spawnCoroutine = StartCoroutine(EnemySpawnerCycle());
    }

    public void StopSpawn()
    {
        StopCoroutine(_spawnCoroutine);
    }

    protected IEnumerator EnemySpawnerCycle()
    {
        while (true)
        {
            SpawnEnemy(Random.Range(_minEnemiesToSpawn, _maxEnemiesToSpawn));
            yield return new WaitForSeconds(Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
        }
    }

    protected void SpawnEnemy(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            var enemyType = GetRandomEnemyType();
            var enemy = _objectPool.Get(enemyType);
            if (enemy != null)
            {
                enemy.GetComponent<ITargetHolder>().SetTarget(_target);
                enemy.GetComponent<IPositionable>().SetPosition(GetEnemySpawnPosition());
                enemy.GetComponent<IResetable>().ResetState();
                enemy.GetComponent<IDisposable>().OnDispose += DisposeEnemyHandler;
            }
        }
    }

    protected EResource GetRandomEnemyType()
    {
        float randomValue = Random.value;
        if (randomValue < _meleeEnemySpawnChance)
        {
            return EResource.EnemyMelee; 
        }
        else
        {
            return EResource.EnemyRange;
        }
    }

    protected void DisposeEnemyHandler(GameObject enemy)
    {
        TransferExperience(enemy);
        DisposeEnemy(enemy);
    }

    protected void TransferExperience(GameObject enemy)
    {
        var killExperience = enemy.GetComponent<IExperienceMaker>().MakeExperience();
        _experienceTaker.TakeExperience(killExperience);
    }

    protected void DisposeEnemy(GameObject enemy)
    {
        enemy.GetComponent<IDisposable>().OnDispose -= DisposeEnemyHandler;
        _objectPool.Return(enemy);
    }

    protected Vector3 GetEnemySpawnPosition()
    {
        var angle = Random.Range(0f, _spawnRaduis);
        var theta = Mathf.Deg2Rad * angle;
        var radius = Random.Range(_minRadius, _maxRadius);
        var spawnVector = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));

        return spawnVector + _target.position;
    }
}
