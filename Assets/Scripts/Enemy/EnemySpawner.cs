using System.Collections;
using UnityEngine;
using Enums;

public class EnemySpawner : MonoBehaviour
{
    private Transform _target;
    private IExperienceTaker _experienceTaker;
    private ObjectPool _objectPool;

    //private bool _spawn = true;
    private float _minRadius = 10f;
    private float _maxRadius = 20f;
    private float _spawnRaduis = 360f;
    private float _minEnemySpawnTime = 1;
    private float _maxEnemySpawnTime = 3;
    private int _minEnemiesToSpawn = 1;
    private int _maxEnemiesToSpawn = 3;
    
    private Coroutine _spawnCoroutine;

    public void Init(Transform playerTransform, IExperienceTaker experienceTaker, ObjectPool objectPool)
    {
        _target = playerTransform;
        _experienceTaker = experienceTaker;
        _objectPool = objectPool;
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawnCoroutine);
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
            var enemy = _objectPool.Get(EResource.Enemy);
            if (enemy != null)
            {
                enemy.GetComponent<ITargetHolder>().SetTarget(_target);
                enemy.GetComponent<IPositionable>().SetPosition(GetEnemySpawnPosition());
                enemy.GetComponent<IResetable>().ResetState();
                enemy.GetComponent<IDisposable>().OnDispose += DisposeEnemyHandler;
            }
        }
    }

    private void DisposeEnemyHandler(GameObject enemy)
    {
        TransferExperience(enemy);
        DisposeEnemy(enemy);
    }

    private void TransferExperience(GameObject enemy)
    {
        var killExperience = enemy.GetComponent<IExperienceMaker>().MakeExperience();
        _experienceTaker.TakeExperience(killExperience);
    }

    private void DisposeEnemy(GameObject enemy)
    {
        enemy.GetComponent<IDisposable>().OnDispose -= DisposeEnemyHandler;
        _objectPool.Return(enemy);
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
