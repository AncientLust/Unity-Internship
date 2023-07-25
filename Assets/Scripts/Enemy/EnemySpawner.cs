using System.Collections;
using UnityEngine;
using Enums;

public class EnemySpawner : MonoBehaviour
{
    private Transform _target;
    private ObjectPool _objectPool;
    private EnemyDisposalManager _disposalManager;

    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 3;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 2;
    private float _meleeEnemySpawnChance = .90f;
    private Coroutine _spawnCoroutine;

    public void Init(Transform playerTransform, ObjectPool objectPool, EnemyDisposalManager disposalManager)
    {
        _target = playerTransform;
        _objectPool = objectPool;
        _disposalManager = disposalManager;
    }

    private void OnDestroy()
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

    private IEnumerator EnemySpawnerCycle()
    {
        while (true)
        {
            SpawnEnemy(Random.Range(_minEnemiesToSpawn, _maxEnemiesToSpawn));
            yield return new WaitForSeconds(Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
        }
    }

    private void SpawnEnemy(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            var enemyType = GetRandomEnemyType();
            var enemy = _objectPool.Get(enemyType);
            if (enemy != null)
            {
                var enemyDisposalSystem = enemy.GetComponent<EnemyDisposalSystem>();
                _disposalManager.SubscribeEnemy(enemyDisposalSystem);
                enemy.transform.position = GetEnemySpawnPosition();
                enemy.GetComponent<IResetable>().ResetState();
            }
        }
    }

    private EResource GetRandomEnemyType()
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

    private Vector3 GetEnemySpawnPosition()
    {
        var angle = Random.Range(0f, _spawnRaduis);
        var theta = Mathf.Deg2Rad * angle;
        var radius = Random.Range(_minRadius, _maxRadius);
        var spawnVector = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));

        return spawnVector + _target.position;
    }
}
