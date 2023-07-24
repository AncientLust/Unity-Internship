using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private EnemyExperienceSystem _experienceSystem;
    private EnemyStatsSystem _statsSystem;
    private EnemyMovementSystem _movementSystem;
    private EnemyHealthSystem _healthSystem;
    private EnemyFacade _enemyFacade;
    private EnemyEffectsSystem _effectSystem;
    private EnemyWeaponSystem _weaponSystem;

    private float _followPlayerDistance = 5f;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        _statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        _movementSystem = gameObject.AddComponent<EnemyMovementSystem>();
        _healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        _enemyFacade = gameObject.AddComponent<EnemyFacade>();
        _effectSystem = gameObject.AddComponent<EnemyEffectsSystem>();
        _weaponSystem = gameObject.AddComponent<EnemyWeaponSystem>();
    }

    public void Init(ObjectPool objectPool, Transform target)
    {
        _statsSystem.Init(_experienceSystem);
        _movementSystem.Init(target, _rigidBody, _statsSystem, _followPlayerDistance);
        _healthSystem.Init(_statsSystem, objectPool);
        _effectSystem.Init(_healthSystem);
        _weaponSystem.Init(_statsSystem, objectPool);

        _enemyFacade.Init(
            _experienceSystem,
            _healthSystem,
            _movementSystem
        );
    }
}
