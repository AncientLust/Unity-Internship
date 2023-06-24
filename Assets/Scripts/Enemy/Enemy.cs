using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IPushiable
{
    private float _stopDistance = 0.35f;
    private float _damagePerSecond = 10;
    private float _levelsPerMinute = 3;
    private int _killExperience = 10;
    private Transform _target;
    private Rigidbody _rigidbody;
    private HealthSystem _healthSystem;
    private StatsSystem _statsSystem;

    private void Awake()
    {
        CacheComponents();
        SetLevelBasedOnGameDuration();
    }

    private void Update()
    {
        ActIfGameRunning();
    }

    private void FixedUpdate()
    {
        ActPhisicallyIfGameRunning();
    }

    private void OnEnable()
    {
        ResetHealth();
        SetLevelBasedOnGameDuration();
    }

    public void ResetHealth()
    {
        _statsSystem.CurrentHealth = _statsSystem.MaxHealth;
        _healthSystem.IsDead = false;
    }

    private void SetLevelBasedOnGameDuration()
    {
        var minutesSceneLoaded = Time.timeSinceLevelLoad / 60.0f;
        var enemyLevel = (int)Mathf.Ceil(minutesSceneLoaded * _levelsPerMinute);
        _statsSystem.SetLevelStats(enemyLevel > 1 ? enemyLevel : 1);
    }

    public void Die()
    {
        _target.GetComponent<ExperienceSystem>().AddExperience(_killExperience * _statsSystem.Level);
        ObjectPool.Instance.Add(gameObject);
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.TakeDamage(damage);
    }

    public void Push(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void ActIfGameRunning()
    {
        if (ShouldAct())
        {
            _healthSystem.Regenerate();
        }
    }

    private bool ShouldAct()
    {
        return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
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

    private void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
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

    private void MoveToPlayer()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        if (distanceToPlayer > _stopDistance)
        {
            Vector3 direction = (_target.position - transform.position).normalized;

            _rigidbody.MovePosition(_rigidbody.position + direction * _statsSystem.MoveSpeed * Time.deltaTime);
        }
    }

    private void RotateToPlayer()
    {
        Vector3 directionToTarget = (_target.position - _rigidbody.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        _rigidbody.MoveRotation(targetRotation);
    }

    private void OnCollisionStay(Collision collision)
    {
        var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isAnotherEnemy = collision.gameObject.GetComponent<Enemy>();

        if (damagable != null && !isAnotherEnemy)
        {
            damagable.TakeDamage(_damagePerSecond * Time.deltaTime * _statsSystem.DamageMultiplier);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
