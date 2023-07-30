using UnityEngine;
using Structs;

public class EnemyMovementSystem : MonoBehaviour
{
    private float _followPlayerDistance;
    private float _slowDownDistance;
    private float _slowDownDistanceMultiplicator = 1.25f;
    private float _baseMoveSpeed = 6f;
    private float _moveSpeed;
    private bool _isInitialized;
    private bool _isEnabled;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private EnemyStatsSystem _statsSystem;
    private EnemyHealthSystem _healthSystem;
    private Transform _target;

    public void Init
    (
        Transform target,
        Rigidbody rigidbody,
        CapsuleCollider collider,
        EnemyStatsSystem enemyStatsSystem,
        EnemyHealthSystem healthSystem,
        float followPlayerDistance
    )
    {
        _target = target;
        _rigidbody = rigidbody;
        _collider = collider;
        _statsSystem = enemyStatsSystem;
        _healthSystem = healthSystem;
        
        _moveSpeed = _baseMoveSpeed;
        _followPlayerDistance = followPlayerDistance;
        _slowDownDistance = _followPlayerDistance * _slowDownDistanceMultiplicator;
        _isInitialized = true;
        _isEnabled = true;
        Subscribe();
    }

    private void OnEnable()
    {
        StartMoving();
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        if (_healthSystem != null) _healthSystem.onDie += StopMoving;
    }

    private void Unsubscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
        if (_healthSystem != null) _healthSystem.onDie -= StopMoving;
    }

    private void FixedUpdate()
    {
        ActPhisically();
    }

    private void ActPhisically()
    {
        if (_isInitialized && _isEnabled)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        var direction = (_target.position - transform.position).normalized;

        if (distanceToPlayer > _slowDownDistance)
        {
            _rigidbody.velocity = direction * _moveSpeed;
        }
        else if (distanceToPlayer > _followPlayerDistance)
        {
            float slowdownFactor = (distanceToPlayer - _followPlayerDistance) / (_slowDownDistance - _followPlayerDistance);
            _rigidbody.velocity = direction * _moveSpeed * slowdownFactor;
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void RotateToPlayer()
    {
        Vector3 directionToTarget = (_target.position - _rigidbody.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            _rigidbody.MoveRotation(targetRotation);
        }
    }

    private void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    public void Push(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void StartMoving()
    {
        if (_isInitialized)
        {
            _isEnabled = true;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }

    private void StopMoving()
    {
        _isEnabled = false;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }
}
