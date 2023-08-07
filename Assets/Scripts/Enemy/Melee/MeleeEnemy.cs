using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private CapsuleCollider _collider;
    private EnemyExperienceSystem _experienceSystem;
    private EnemyStatsSystem _statsSystem;
    private MeleeEnemyAttackSystem _attackSystem;
    private EnemyMovementSystem _movementSystem;
    private EnemyHealthSystem _healthSystem;
    private EnemyFacade _enemyFacade;
    private EnemyEffectsSystem _effectSystem;
    private EnemyDisposalSystem _enemyDisposalSystem;
    private EnemyAnimationSystem _animationSystem;

    private float _followPlayerDistance = 0.75f;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _collider = gameObject.GetComponent<CapsuleCollider>();
        _experienceSystem = gameObject.AddComponent<EnemyExperienceSystem>();
        _statsSystem = gameObject.AddComponent<EnemyStatsSystem>();
        _attackSystem = gameObject.AddComponent<MeleeEnemyAttackSystem>();
        _movementSystem = gameObject.AddComponent<EnemyMovementSystem>();
        _healthSystem = gameObject.AddComponent<EnemyHealthSystem>();
        _enemyFacade = gameObject.AddComponent<EnemyFacade>();
        _effectSystem = gameObject.AddComponent<EnemyEffectsSystem>();
        _enemyDisposalSystem = gameObject.AddComponent<EnemyDisposalSystem>();
        _animationSystem = gameObject.AddComponent<EnemyAnimationSystem>();
    }

    public void Init(IAudioPlayer iAudioPlayer, Transform target)
    {
        _statsSystem.Init(_experienceSystem);
        _movementSystem.Init(target, _rigidBody, _collider, _statsSystem, _healthSystem, _followPlayerDistance);
        _healthSystem.Init(_statsSystem, iAudioPlayer);
        _attackSystem.Init(_statsSystem);
        _effectSystem.Init(_healthSystem);
        _enemyDisposalSystem.Init(_healthSystem);
        _animationSystem.Init(_healthSystem);

        _enemyFacade.Init(
            _experienceSystem,
            _healthSystem,
            _movementSystem
        );
    }
}
