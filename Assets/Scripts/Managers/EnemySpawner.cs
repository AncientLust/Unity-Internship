using System.Collections;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] bool spawn = true;
    [SerializeField] float minRadius = 10f;
    [SerializeField] float maxRadius = 15f;
    [SerializeField] float minEnemySpawnTime = 1;
    [SerializeField] float maxEnemySpawnTime = 2;
    [SerializeField] GameObject enemyObject;
    [SerializeField] GameObject targetObject;

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
            enemy.gameObject.GetComponent<Enemy>().SetTarget(targetObject);
            enemy.transform.position = GetEnemySpawnPosition();
            enemy.transform.rotation = enemyObject.transform.rotation;
            enemy.gameObject.SetActive(true);
        }
    }

    private Vector3 GetEnemySpawnPosition()
    {
        float angle = Random.Range(0f, 360f);
        float theta = Mathf.Deg2Rad * angle;
        float radius = Random.Range(minRadius, maxRadius);
        return new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
    }
}
