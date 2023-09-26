using UnityEngine;
using Structs;
using UnityEngine.AI;

public class EnemyMovementSystem : MonoBehaviour
{
    private float _baseMoveSpeed = 3.5f;
    private bool _isInitialized;
    private bool _isEnabled;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;
    private EnemyStatsSystem _statsSystem;
    private EnemyHealthSystem _healthSystem;
    private NavMeshAgent _navMeshAgent;
    private Transform _target;

    public void Init
    (
        Transform target,
        Rigidbody rigidbody,
        CapsuleCollider collider,
        EnemyStatsSystem enemyStatsSystem,
        EnemyHealthSystem healthSystem,
        NavMeshAgent navMeshAgent
    )
    {
        _target = target;
        _rigidbody = rigidbody;
        _collider = collider;
        _statsSystem = enemyStatsSystem;
        _healthSystem = healthSystem;
        _navMeshAgent = navMeshAgent;

        _navMeshAgent.speed = _baseMoveSpeed;
        _isInitialized = true;
        _isEnabled = true;

        StartMoving();
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            ResetMoveSpeed();
            StartMoving();
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        _healthSystem.onDie += StopMoving;
    }

    private void Unsubscribe()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
        _healthSystem.onDie -= StopMoving;
    }

    private void Update()
    {
        SetDestination();
        LookAtTarget();
    }
     
    private void SetDestination()
    {
        if (_isEnabled)
        {
            _navMeshAgent.SetDestination(_target.position);
        }
    }

    public void SetPosition(Vector3 position)
    {
        _navMeshAgent.enabled = false;
        transform.position = position;
        _navMeshAgent.enabled = true;
    }

    private void LookAtTarget()
    {
        if (!_isEnabled || _navMeshAgent.remainingDistance >= _navMeshAgent.stoppingDistance)
            return;

        Vector3 directionToTarget = (_target.position - transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _navMeshAgent.angularSpeed * Time.deltaTime);
        }
    }

    private void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _navMeshAgent.speed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    private void ResetMoveSpeed()
    {
        _navMeshAgent.speed = _baseMoveSpeed;
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
            _navMeshAgent.enabled = true;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }

    private void StopMoving()
    {
        _isEnabled = false;
        _navMeshAgent.enabled = false;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }
}
