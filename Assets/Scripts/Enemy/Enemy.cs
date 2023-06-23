using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private float _stopDistance = 0.25f;
    private float _damage = 10;
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
        _statsSystem.SetLevel(enemyLevel > 1 ? enemyLevel : 1);
    }

    public void Die()
    {
        _target.GetComponent<ExperienceSystem>().AddExperience(_killExperience * _statsSystem.Level);
        gameObject.SetActive(false);
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

    private void ActIfGameRunning()
    {
        if (!GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            ResetVelosity();
            return;
        }

        _healthSystem.Regenerate();
        MoveIfPlayerAlive();
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
            transform.position = Vector3.MoveTowards(transform.position, _target.position, _statsSystem.MoveSpeed * Time.deltaTime);
        }

        transform.LookAt(_target.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Read about comparison between collision and trigger
        var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isAnotherEnemy = collision.gameObject.GetComponent<Enemy>();

        if (damagable != null && !isAnotherEnemy)
        {
            damagable.TakeDamage(_damage * _statsSystem.PowerMultiplier);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
