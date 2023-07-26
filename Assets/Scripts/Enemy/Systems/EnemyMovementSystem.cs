using UnityEngine;
using Structs;

public class EnemyMovementSystem : MonoBehaviour
{
    private float _followPlayerDistance;
    private float _baseMoveSpeed = 6f;
    private float _moveSpeed;
    private Rigidbody _rigidbody;
    private EnemyStatsSystem _statsSystem;
    private Transform _target;
    private bool _isInitialized;

    public void Init
    (
        Transform target,
        Rigidbody rigidbody, 
        EnemyStatsSystem enemyStatsSystem, 
        float followPlayerDistance
    )
    {
        _target = target;
        _rigidbody = rigidbody;
        _statsSystem = enemyStatsSystem;
        _moveSpeed = _baseMoveSpeed;
        _followPlayerDistance = followPlayerDistance;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void Unsubscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void FixedUpdate()
    {
        ActPhisically();
    }

    private void ActPhisically()
    {
        if (_isInitialized)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (distanceToPlayer > _followPlayerDistance)
        {
            var direction = (_target.position - transform.position).normalized;
            _rigidbody.velocity = direction * _moveSpeed;
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
}
