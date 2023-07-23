using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        var statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        var attackSystem = gameObject.AddComponent<MeleeEnemyAttackSystem>();
        var movementSystem = gameObject.AddComponent<MeleeEnemyMovementSystem>();
        var healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        var enemyFacade = gameObject.AddComponent<EnemyFacade>();
        var effectSystem = gameObject.AddComponent<EnemyEffectsSystem>();

        statsSystem.Init(experienceSystem);
        movementSystem.Init(rigidBody, statsSystem);
        healthSystem.Init(statsSystem);
        attackSystem.Init(statsSystem);
        effectSystem.Init(healthSystem);

        enemyFacade.Init(
            experienceSystem,
            healthSystem,
            movementSystem
        );
    }
}
