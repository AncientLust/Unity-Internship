using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private CapsuleCollider _collider;
    private EnemyExperienceSystem _experienceSystem;
    private EnemyStatsSystem _statsSystem;
    private EnemyMovementSystem _movementSystem;
    private EnemyHealthSystem _healthSystem;
    private EnemyFacade _enemyFacade;
    private EnemyEffectsSystem _effectSystem;
    private EnemyWeaponSystem _weaponSystem;
    private EnemyAnimationSystem _animationSystem;
    private EnemyDropSystem _dropSystem;

    private float _followPlayerDistance = 5f;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<CapsuleCollider>();
        _experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        _statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        _movementSystem = gameObject.AddComponent<EnemyMovementSystem>();
        _healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        _enemyFacade = gameObject.AddComponent<EnemyFacade>();
        _effectSystem = gameObject.AddComponent<EnemyEffectsSystem>();
        _weaponSystem = gameObject.AddComponent<EnemyWeaponSystem>();
        _animationSystem = gameObject.AddComponent<EnemyAnimationSystem>();
        _dropSystem = gameObject.AddComponent<EnemyDropSystem>();
    }

    public void Init(IAudioPlayer iAudioPlayer, ObjectPool objectPool, Transform target, GameSettings gameSettings)
    {
        _statsSystem.Init(_experienceSystem);
        _movementSystem.Init(target, _rigidBody, _collider, _statsSystem, _healthSystem, _followPlayerDistance);
        _healthSystem.Init(_statsSystem, iAudioPlayer);
        _effectSystem.Init(_healthSystem, gameSettings);
        _weaponSystem.Init(_statsSystem, objectPool, _healthSystem, iAudioPlayer);
        _animationSystem.Init(_healthSystem);
        _dropSystem.Init(objectPool, _healthSystem);

        _enemyFacade.Init(
            _experienceSystem,
            _healthSystem,
            _movementSystem
        );
    }
}
