using UnityEngine;

public class Enemy : MonoBehaviour, IPushiable, ISaveable
{
    private float _stopDistance = 0.35f;
    private float _damagePerSecond = 10;
    private float _levelsPerMinute = 3;
    private int _killExperience = 10;
    private Rigidbody _rigidbody;
    private HealthSystem _healthSystem;
    private StatsSystem _statsSystem;
    private ExperienceSystem _experienceSystem;
    
    private Transform _target; // Must be injected
    private ExperienceSystem _playerExperienceSystem; // Must be injected

    public void Init(Transform target, ExperienceSystem playerExperienceSystem)
    {
        _target = target;
        _playerExperienceSystem = playerExperienceSystem;
    }

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        SetLevelBasedOnGameDuration();
    }

    private void FixedUpdate()
    {
        ActPhisicallyIfGameRunning();
    }

    private void OnEnable()
    {
        _healthSystem.OnDie += Die;
    }

    private void OnDisable()
    {
        _healthSystem.OnDie -= Die;
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

    public void Reset()
    {
        SetLevelBasedOnGameDuration();
    }

    private void SetLevelBasedOnGameDuration()
    {
        var minutesSceneLoaded = Time.timeSinceLevelLoad / 60.0f;
        var enemyLevel = (int)Mathf.Ceil(minutesSceneLoaded * _levelsPerMinute);
        _experienceSystem.SetLevel(enemyLevel);
    }

    private void Die()
    {
        _playerExperienceSystem.AddExperience(_killExperience * _experienceSystem.GetLevel());
        ObjectPool.Instance.Return(gameObject);
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _experienceSystem = GetComponent<EnemyExperienceSystem>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    public void Push(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
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

        if (damagable != null && !isAnotherEnemy && ShouldAct())
        {
            damagable.TakeDamage(_damagePerSecond * Time.deltaTime);
            //damagable.TakeDamage(_damagePerSecond * Time.deltaTime * _statsSystem.DamageMultiplier);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public EntityData CaptureState()
    {
        EntityData data = new EntityData();
        //data.health = _statsSystem.CurrentHealth;
        data.position = transform.position;
        //data.level = _experienceSystem.Level;

        return data;
    }

    public void LoadState(EntityData data)
    {
        transform.position = data.position;
        //_statsSystem.CurrentHealth = data.health;
        //_experienceSystem.Level = data.level;
    }
}
