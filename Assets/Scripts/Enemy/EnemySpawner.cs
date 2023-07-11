using System.Collections;
using UnityEngine;
using Enums;

public class EnemySpawner : MonoBehaviour
{
    private Transform _playerTransform;
    private IExperienceSystem _playerExperienceSystem;
    private ObjectPool _objectPool;

    //private bool _spawn = true;
    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 2;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 3;
    
    private Coroutine _spawnCoroutine;

    //public void Init(ObjectPool objectPool)
    //{
    //    _objectPool = objectPool;
    //}

    public void Init(Transform playerTransform, IExperienceSystem playerExperienceSystem)
    {
        _playerTransform = playerTransform;
        _playerExperienceSystem = playerExperienceSystem;
    }

    private void Start()
    {
        _objectPool = new ObjectPool();
    }

    private void OnDestroy()
    {
        //_spawn = false;
        StopAllCoroutines();
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
            if (_objectPool != null)
            {
                GameObject enemy = ObjectPool.Instance.Get(EResource.Enemy);
                if (enemy != null)
                { 
                    enemy.GetComponent<EnemyFacade>().Init2(_playerTransform, _playerExperienceSystem); // Must be refactored
                    enemy.GetComponent<EnemyHealthSystem>().ResetHealth();
                    enemy.transform.position = GetEnemySpawnPosition();
                }
            }
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        var angle = Random.Range(0f, _spawnRaduis);
        var theta = Mathf.Deg2Rad * angle;
        var radius = Random.Range(_minRadius, _maxRadius);
        var spawnVector = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));

        return spawnVector + _playerTransform.position;
    }
}
