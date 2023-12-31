using System.Collections;
using UnityEngine;
using Enums;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnAngle = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 2;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 2;
    private float _meleeEnemySpawnChance = .75f;
    private int _baseEnemyLevel = 1;
    private int _enemyLevel;
    private Coroutine _spawnCoroutine;

    private Transform _target;
    private ObjectPool _objectPool;
    private EnemyDisposalManager _disposalManager;
    private LevelProgressManager _levelProgressManager;
     
    public void Init
    (
        Transform targetTransform, 
        ObjectPool objectPool, 
        EnemyDisposalManager disposalManager, 
        LevelProgressManager levelProgressManager
    )
    {
        _target = targetTransform;
        _objectPool = objectPool;
        _disposalManager = disposalManager;
        _levelProgressManager = levelProgressManager;

        _enemyLevel = _baseEnemyLevel;
        _levelProgressManager.onGameLevelChanged += SetEnemyLevel;
    }

    private void OnDestroy()
    {
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
        _levelProgressManager.onGameLevelChanged -= SetEnemyLevel;
    }

    public void StartSpawn()
    {
        _spawnCoroutine = StartCoroutine(EnemySpawnerCycle());
    }

    public void StopSpawn()
    {
        StopCoroutine(_spawnCoroutine);
    }

    private void SetEnemyLevel(int enemyLevel)
    {
        _enemyLevel = enemyLevel;
    }

    public void ResetEnemyLevel()
    {
        _enemyLevel = _baseEnemyLevel;
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
                var healthSystem = enemy.GetComponent<EnemyHealthSystem>();
                _disposalManager.SubscribeEnemy(healthSystem);
                enemy.GetComponent<IResetable>().ResetState();
                enemy.GetComponent<EnemyExperienceSystem>().SetLevel(_enemyLevel);
                enemy.GetComponent<EnemyMovementSystem>().SetPosition(GetEnemySpawnPosition());
            }
        }
    }

    private EResource GetRandomEnemyType()
    {
        if (Random.value < _meleeEnemySpawnChance)
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
        while (true)
        {
            var angle = Random.Range(0f, _spawnAngle);
            var theta = Mathf.Deg2Rad * angle;
            var radius = Random.Range(_minRadius, _maxRadius);
            var spawnVector = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
            var spawnVectorRelativeToTarget = spawnVector + _target.position;
            
            NavMeshHit hit;
            if (NavMesh.SamplePosition(spawnVectorRelativeToTarget, out hit, 1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
    }
}
