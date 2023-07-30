using System.Collections;
using UnityEngine;

public class EnemyAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private EnemyHealthSystem _enemyHealthSystem;

    public void Init
    (
        //Rigidbody rigidbody,
        EnemyHealthSystem enemyHealthSystem
    )
    {
        _animator = GetComponent<Animator>();
        _enemyHealthSystem = enemyHealthSystem;
        //_rigidbody = rigidbody;

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
