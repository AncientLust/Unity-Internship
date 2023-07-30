using UnityEngine;

public class EnemyAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private EnemyHealthSystem _enemyHealthSystem;

    public void Init(EnemyHealthSystem enemyHealthSystem)
    {
        _animator = GetComponent<Animator>();
        _enemyHealthSystem = enemyHealthSystem;

        if (_enemyHealthSystem != null) _enemyHealthSystem.onDie += PlayDeath;
    }

    private void OnEnable()
    {
        if (_enemyHealthSystem != null) _enemyHealthSystem.onDie += PlayDeath;
    }

    private void OnDisable()
    {
        if (_enemyHealthSystem != null) _enemyHealthSystem.onDie -= PlayDeath;
    }

    private void PlayDeath()
    {
        _animator.SetTrigger("deathTrigger");
    }
}
