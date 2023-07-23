using UnityEngine;
using Structs;

public class MeleeEnemyMovementSystem : MonoBehaviour, IEnemyMovementSystem
{
    protected float _stopDistance = 0.35f;
    protected float _baseMoveSpeed = 6f;
    protected float _moveSpeed;
    protected bool _mustMove = true;

    protected Rigidbody _rigidbody;
    protected EnemyStatsSystem _statsSystem;

    protected Transform _target;

    public void Init(Rigidbody rigidbody, EnemyStatsSystem enemyStatsSystem)
    {
        _rigidbody = rigidbody;
        _statsSystem = enemyStatsSystem;
        _moveSpeed = _baseMoveSpeed;
        Subscribe();
    }

    protected void OnEnable()
    {
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void Subscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    protected void Unsubscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    protected void FixedUpdate()
    {
        ActPhisicallyIfGameRunning();
    }

    protected void ActPhisicallyIfGameRunning()
    {
        if (_mustMove)
        {
            MoveIfPlayerAlive();
        }
        else
        {
            ResetVelosity();
        }
    }

    protected void MoveIfPlayerAlive()
    {
        if (true)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
    }

    protected void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (distanceToPlayer > _stopDistance)
        {
            Vector3 direction = (_target.position - transform.position).normalized;

            _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.deltaTime);
        }
    }

    protected void RotateToPlayer()
    {
        Vector3 directionToTarget = (_target.position - _rigidbody.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _rigidbody.MoveRotation(targetRotation);
    }

    protected void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    protected void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    public void Push(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
