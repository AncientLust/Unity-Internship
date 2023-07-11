using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var levelSystem = gameObject.AddComponent<EnemyLevelSystem>();
        var statsSystem = gameObject.AddComponent<EnemyStatsSystem>();

        var movementSystem = gameObject.AddComponent<EnemyMovementSystem>();
        var healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        var facade = gameObject.AddComponent<EnemyFacade>();

        statsSystem.Init(levelSystem);
        movementSystem.Init(rigidBody, statsSystem);
        healthSystem.Init(statsSystem);

        facade.Init(
            levelSystem,
            healthSystem,
            movementSystem
        );
    }
}
