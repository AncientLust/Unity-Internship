using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        var statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        var attackSystem = gameObject.AddComponent<EnemyAttackSystem>();
        var movementSystem = gameObject.AddComponent<EnemyMovementSystem>();
        var healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        //var enemySubsystems = gameObject.AddComponent<EnemySubsystems>();

        statsSystem.Init(experienceSystem);
        movementSystem.Init(rigidBody, statsSystem);
        healthSystem.Init(statsSystem);
        attackSystem.Init(statsSystem);

        //enemySubsystems.Init(
        //    experienceSystem,
        //    healthSystem,
        //    movementSystem
        //);
    }
}
