using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private bool _spawn = true;
    [SerializeField] private Transform _playerTransform;

    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 2;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 3;
    private const string _enemy = "Enemy";
    
    private void Start()
    {
        StartCoroutine(EnemySpawnerCycle());
    }

    private IEnumerator EnemySpawnerCycle()
    {
        while (true)
        {
            if (_spawn && GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused)
            {
                SpawnEnemy(Random.Range(_minEnemiesToSpawn, _maxEnemiesToSpawn));
            }

            yield return new WaitForSeconds(Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
        }
    }

    private void SpawnEnemy(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = ObjectPool.Instance.Get(_enemy);
            if (enemy != null)
            { 
                enemy.GetComponent<Enemy>().SetTarget(_playerTransform);
                enemy.transform.position = GetEnemySpawnPosition();
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
