using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private bool _spawn = true;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _target;

    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 2;
    private float _doubleSpawnChance = 0.33f;

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
                if (Random.value <= _doubleSpawnChance)
                {
                    SpawnEnemy();
                    SpawnEnemy();
                }

                SpawnEnemy();
            }

            yield return new WaitForSeconds(Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = EnemyPool.SharedInstance.GetPooledObject();
        if (enemy != null)
        {
            var minutesSinceSceneLoaded = (int)(Time.timeSinceLevelLoad / 60.0f);
            enemy.gameObject.GetComponent<StatsSystem>().SetLevel(minutesSinceSceneLoaded);
            enemy.gameObject.GetComponent<Enemy>().SetTarget(_target);
            enemy.transform.position = GetEnemySpawnPosition();
            enemy.transform.rotation = enemy.transform.rotation;
            enemy.gameObject.SetActive(true);
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        var angle = Random.Range(0f, _spawnRaduis);
        var theta = Mathf.Deg2Rad * angle;
        var radius = Random.Range(_minRadius, _maxRadius);
        var spawnVector = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));

        return spawnVector + _target.transform.position;
    }
}
