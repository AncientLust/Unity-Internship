using UnityEngine;

public class EnemyAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private EnemyHealthSystem _enemyHealthSystem;
    private bool _isInitialized;

    public void Init(EnemyHealthSystem enemyHealthSystem)
    {
        _animator = GetComponent<Animator>();
        _enemyHealthSystem = enemyHealthSystem;
        _isInitialized = true;

        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _enemyHealthSystem.onDie += PlayDeath;
    }

    private void Unsubscribe()
    {
        _enemyHealthSystem.onDie -= PlayDeath;
    }

    private void PlayDeath()
    {
        _animator.SetTrigger("deathTrigger");
    }
}
