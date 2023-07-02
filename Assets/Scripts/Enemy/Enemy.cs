using UnityEngine;

public class Enemy : MonoBehaviour, ISaveable
{
    private float _levelsPerMinute = 3;
    private int _killExperience = 10;

    private EnemyHealthSystem _healthSystem; // Must be injected
    private EnemyExperienceSystem _experienceSystem; // Must be injected
    private EnemyMovementSystem _enemyMovementSystem; // Must be injected
    private PlayerExperienceSystem _playerExperienceSystem; // Must be injected

    public void Init(Transform target, PlayerExperienceSystem playerExperienceSystem)
    {
        _enemyMovementSystem.SetTarget(target);
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

    private void OnEnable()
    {
        _healthSystem.OnDie += Die;
    }

    private void OnDisable()
    {
        _healthSystem.OnDie -= Die;
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
        _experienceSystem = GetComponent<EnemyExperienceSystem>();
        _healthSystem = GetComponent<EnemyHealthSystem>();
        _enemyMovementSystem = GetComponent<EnemyMovementSystem>();
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
