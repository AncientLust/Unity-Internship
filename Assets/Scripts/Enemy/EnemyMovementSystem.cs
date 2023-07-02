using UnityEngine;

public class EnemyMovementSystem : MonoBehaviour, IPushiable
{
    private float _stopDistance = 0.35f;
    private float _baseMoveSpeed = 6f;
    private float _moveSpeed;
    
    private Rigidbody _rigidbody;
    private Transform _target; // Must be injected
    private EnemyStatsSystem _statsSystem; // Must be injected

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }

    private void OnEnable()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void OnDisable()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void FixedUpdate()
    {
        ActPhisicallyIfGameRunning();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<EnemyStatsSystem>();
    }

    private void ActPhisicallyIfGameRunning()
    {
        if (ShouldAct())
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
        if (_target.gameObject.activeInHierarchy)
        {
            MoveToPlayer();
            RotateToPlayer();
        }
        else
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    private bool ShouldAct()
    {
        return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    }

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

    private void ApplyLevelUpMultipliers(EnemyStatsMultipliers multipliers)
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
}
