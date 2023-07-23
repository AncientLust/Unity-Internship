using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        var statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        var attackSystem = gameObject.AddComponent<MeleeEnemyAttackSystem>();
        var movementSystem = gameObject.AddComponent<RangeEnemyMovementSystem>();
        var healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        var enemyFacade = gameObject.AddComponent<EnemyFacade>();
        var effectSystem = gameObject.AddComponent<EnemyEffectsSystem>();
        //var weaponSystem = gameObject.AddComponent<EnemyWeaponSystem>();

        statsSystem.Init(experienceSystem);
        movementSystem.Init(rigidBody, statsSystem);
        healthSystem.Init(statsSystem);
        attackSystem.Init(statsSystem);
        effectSystem.Init(healthSystem);
        //weaponSystem.Init();

        enemyFacade.Init(
            experienceSystem,
            healthSystem,
            movementSystem
        );
    }
}
