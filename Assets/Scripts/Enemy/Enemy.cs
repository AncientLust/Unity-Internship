using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IPushiable, ISaveable
{
    private float _stopDistance = 0.35f;
    private float _damagePerSecond = 10;
    private float _levelsPerMinute = 3;
    private int _killExperience = 10;
    private Transform _target;
    private Rigidbody _rigidbody;
    private HealthSystem _healthSystem;
    private StatsSystem _statsSystem;
    private ExperienceSystem _experienceSystem;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
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

    public void Init()
    {
        //ResetHealth();
        SetLevelBasedOnGameDuration();
    }

    private bool ShouldAct()
    {
        return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    }

    private void ActIfGameRunning()
    {
        if (ShouldAct())
        {
            _healthSystem.Regenerate();
        }
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

    private void SetLevelBasedOnGameDuration()
    {
        var minutesSceneLoaded = Time.timeSinceLevelLoad / 60.0f;
        var enemyLevel = (int)Mathf.Ceil(minutesSceneLoaded * _levelsPerMinute);
        _experienceSystem.Level = enemyLevel;
    }

    public void Die()
    {
        _target.GetComponent<ExperienceSystem>().AddExperience(_killExperience * _experienceSystem.Level);
        ObjectPool.Instance.Return(gameObject);
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
        _experienceSystem = GetComponent<ExperienceSystem>();
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.TakeDamage(damage);
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
        data.level = _experienceSystem.Level;

        return data;
    }

    public void LoadState(EntityData data)
    {
        transform.position = data.position;
        //_statsSystem.CurrentHealth = data.health;
        _experienceSystem.Level = data.level;
    }
}
