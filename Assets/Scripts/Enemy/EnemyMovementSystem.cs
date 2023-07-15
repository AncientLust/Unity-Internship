using UnityEngine;
using Structs;

public class EnemyMovementSystem : MonoBehaviour, IPushiable, ITargetHolder, IPositionable
{
    private float _stopDistance = 0.35f;
    private float _baseMoveSpeed = 6f;
    private float _moveSpeed;
    private bool _mustMove = true;
    
    private Rigidbody _rigidbody;
    private EnemyStatsSystem _statsSystem;

    private Transform _target; 

    public void Init(Rigidbody rigidbody, EnemyStatsSystem enemyStatsSystem)
    {
        _rigidbody = rigidbody;
        _statsSystem = enemyStatsSystem;

        _moveSpeed = _baseMoveSpeed;
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
        ActPhisicallyIfGameRunning();
    }

    private void ActPhisicallyIfGameRunning()
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

    private void MoveIfPlayerAlive()
    {
        //if (_target.gameObject.activeInHierarchy)
        if (true)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
        //else
        //{
        //    _rigidbody.angularVelocity = Vector3.zero;
        //}
    }

    //private bool ShouldAct()
    //{
    //    //return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    //    return true;
    //}

    private void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (distanceToPlayer > _stopDistance)
        {
            Vector3 direction = (_target.position - transform.position).normalized;

            _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.deltaTime);
        }
    }

    private void RotateToPlayer()
    {
        Vector3 directionToTarget = (_target.position - _rigidbody.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _rigidbody.MoveRotation(targetRotation);
    }

    private void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    private void ResetVelosity()
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
