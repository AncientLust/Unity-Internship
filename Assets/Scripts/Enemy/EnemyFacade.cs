using System;
using UnityEngine;

public class EnemyFacade : MonoBehaviour, 
    IResetable, IPushiable, IExperienceMaker, IDamageable, IDisposable, ITargetHolder, IPositionable
{
    private EnemyHealthSystem _healthSystem;
    private EnemyExperienceSystem _experienceSystem;
    private EnemyMovementSystem _movementSystem;

    public event Action<GameObject> OnDispose;

    public void Init(
        EnemyExperienceSystem experienceSystem,
        EnemyHealthSystem healthSystem,
        EnemyMovementSystem movementSystem
    )
    {
        _experienceSystem = experienceSystem;
        _healthSystem = healthSystem;
        _movementSystem = movementSystem;

        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_healthSystem != null) _healthSystem.OnDie += () => OnDispose.Invoke(gameObject);
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.OnDie -= () => OnDispose.Invoke(gameObject);
    }

    public float MakeExperience()
    {
        return _experienceSystem.MakeExperience();
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.ReduceHealth(damage);
    }

    public void ResetState()
    {
        _experienceSystem.ResetLevel();
        _healthSystem.RestoreHealth();
    }

    public void SetTarget(Transform target)
    {
        _movementSystem.SetTarget(target);
    }

    public void Push(Vector3 force)
    {
        _movementSystem.Push(force);
    }

    public void SetPosition(Vector3 position)
    {
        _movementSystem.SetPosition(position);
    }







    //public EntityData CaptureState()
    //{
    //    EntityData data = new EntityData();
    //    //data.health = _statsSystem.CurrentHealth;
    //    data.position = transform.position;
    //    //data.level = _experienceSystem.Level;

    //    return data;
    //}



    //public void LoadState(EntityData data)
    //{
    //    transform.position = data.position;
    //    //_statsSystem.CurrentHealth = data.health;
    //    //_experienceSystem.Level = data.level;
    //}
}
