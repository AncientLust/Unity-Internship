using System;
using UnityEngine;

public class EnemyFacade : MonoBehaviour, 
    IResetable, IPushiable, IExperienceMaker, IDamageable, IDisposable, ITargetHolder, IPositionable
{
    private EnemyHealthSystem _healthSystem;
    private EnemyExperienceSystem _experienceSystem;
    private IEnemyMovementSystem _iEnemyMovementSystem;

    public event Action<GameObject> OnDispose;

    public void Init(
        EnemyExperienceSystem experienceSystem,
        EnemyHealthSystem healthSystem,
        IEnemyMovementSystem movementSystem
    )
    {
        _experienceSystem = experienceSystem;
        _healthSystem = healthSystem;
        _iEnemyMovementSystem = movementSystem;

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
        _iEnemyMovementSystem.SetTarget(target);
    }

    public void Push(Vector3 force)
    {
        _iEnemyMovementSystem.Push(force);
    }

    public void SetPosition(Vector3 position)
    {
        _iEnemyMovementSystem.SetPosition(position);
    }
}
