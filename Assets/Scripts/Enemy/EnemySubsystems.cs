using UnityEngine;

public class EnemySubsystems : MonoBehaviour, ISaveable
{
    //private float _levelsPerMinute = 3;

    //private EnemyHealthSystem _healthSystem; // Must be injected
    //private EnemyExperienceSystem _experienceSystem; // Must be injected
    //private EnemyMovementSystem _movementSystem; // Must be injected
    //private IExperienceSystem _playerExperienceSystem; // Must be injected

    //public void Init(
    //    EnemyExperienceSystem experienceSystem,
    //    EnemyHealthSystem healthSystem
    //    //EnemyMovementSystem movementSystem
    //)
    //{
    //    //_experienceSystem = experienceSystem;
    //    //_healthSystem = healthSystem;
    //    //_movementSystem = movementSystem;
    //}

    //public void ResetState()
    //{
    //    _healthSystem.ResetHealth();
    //    _experienceSystem.ResetLevel();
    //}










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
