using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private ObjectPool _objectPool;

    private bool _spawn = true;
    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 2;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 3;
    private const string _enemy = "Enemy";
    
    //public void Init(ObjectPool objectPool)
    //{
    //    _objectPool = objectPool;
    //}

    private void Start()
    {
        _objectPool = new ObjectPool();
        StartCoroutine(EnemySpawnerCycle());
    }

    private void OnDestroy()
    {
        _spawn = false;
        StopAllCoroutines();
    }

    private IEnumerator EnemySpawnerCycle()
    {
        while (_spawn)
        {
            if (GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused)
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
            if (_objectPool != null)
            {
                GameObject enemy = ObjectPool.Instance.Get(_enemy);
                if (enemy != null)
                { 
                    enemy.GetComponent<Enemy>().Init(_player.transform, _player.GetComponent<PlayerExperienceSystem>());
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

        return spawnVector + _player.transform.position;
    }
}
