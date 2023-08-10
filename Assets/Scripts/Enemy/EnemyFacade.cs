using UnityEngine;

public class EnemyFacade : MonoBehaviour, 
    IResetable, IPushiable, IDamageable, IExperienceMaker
{
    private EnemyHealthSystem _healthSystem;
    private EnemyExperienceSystem _experienceSystem;
    private EnemyMovementSystem _enemyMovementSystem;

    public void Init(
        EnemyExperienceSystem experienceSystem,
        EnemyHealthSystem healthSystem,
        EnemyMovementSystem movementSystem
    )
    {
        _experienceSystem = experienceSystem;
        _healthSystem = healthSystem;
        _enemyMovementSystem = movementSystem;
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.ReduceHealth(damage);
    }

    public void ResetState()
    {
        _healthSystem.ResetHealth();
    }

    public void Push(Vector3 force)
    {
        _enemyMovementSystem.Push(force);
    }

    public float MakeExperience()
    {
        return _experienceSystem.GetExperience();
    }
}
