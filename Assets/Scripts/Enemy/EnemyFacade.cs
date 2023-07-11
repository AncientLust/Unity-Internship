using UnityEngine;

public class EnemyFacade : MonoBehaviour, ISaveable
{
    private float _levelsPerMinute = 3;
    private int _killExperience = 10;

    private EnemyHealthSystem _healthSystem; // Must be injected
    private EnemyLevelSystem _levelSystem; // Must be injected
    private EnemyMovementSystem _movementSystem; // Must be injected
    private IExperienceSystem _playerExperienceSystem; // Must be injected

    public void Init(
        EnemyLevelSystem levelSystem, 
        EnemyHealthSystem healthSystem, 
        EnemyMovementSystem movementSystem
    )
    {
        _levelSystem = levelSystem;
        _healthSystem = healthSystem;
        _movementSystem = movementSystem;

        _healthSystem.OnDie += Die;
    }

    private void OnDisable()
    {
        _healthSystem.OnDie -= Die;
    }

    public void Init2(
        Transform target, // Get rid of this, use interface
        IExperienceSystem playerExperienceSystem // Get rid of this use interface
    )
    {
        _movementSystem.SetTarget(target);
        _playerExperienceSystem = playerExperienceSystem;
        _healthSystem.OnDie += Die;
    }

    private void Start()
    {
        SetLevelBasedOnGameDuration();
    }

    public void Reset()
    {
        SetLevelBasedOnGameDuration();
    }

    private void SetLevelBasedOnGameDuration()
    {
        var minutesSceneLoaded = Time.timeSinceLevelLoad / 60.0f;
        var enemyLevel = (int)Mathf.Ceil(minutesSceneLoaded * _levelsPerMinute);
        _levelSystem.SetLevel(enemyLevel);
    }

    private void Die()
    {
        _playerExperienceSystem.AddExperience(_killExperience * _levelSystem.GetLevel());
        ObjectPool.Instance.Return(gameObject);
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
