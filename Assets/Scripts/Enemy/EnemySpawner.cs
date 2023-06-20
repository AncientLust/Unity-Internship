using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private bool spawn = true;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject target;

    private float minRadius = 10f;
    private float maxRadius = 15f;
    private float spawnRaduis = 360f;
    private float minEnemySpawnTime = 1;
    private float maxEnemySpawnTime = 2;

    private void Start()
    {
        StartCoroutine(EnemySpawnerTimer());
    }

    private IEnumerator EnemySpawnerTimer()
    {
        while (true)
        {
            if (spawn && GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused)
            {
                SpawnEnemy();
                
            }

            yield return new WaitForSeconds(Random.Range(minEnemySpawnTime, maxEnemySpawnTime));
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = EnemyPool.SharedInstance.GetPooledObject();
        if (enemy != null)
        {
            enemy.gameObject.GetComponent<Enemy>().InitHealth();
            enemy.gameObject.GetComponent<Enemy>().SetTarget(target);
            enemy.transform.position = GetEnemySpawnPosition();
            enemy.transform.rotation = enemy.transform.rotation;
            enemy.gameObject.SetActive(true);
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        float angle = Random.Range(0f, spawnRaduis);
        float theta = Mathf.Deg2Rad * angle;
        float radius = Random.Range(minRadius, maxRadius);
        return new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
    }
}
