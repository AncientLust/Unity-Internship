using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private PlayerHealthSystem _healthSystem;
    private float _velocityTreshhold = 1;
    private bool _isInitialized;

    public void Init(Rigidbody rigidbody, PlayerHealthSystem healthSystem)
    {
        _animator = GetComponent<Animator>();
        _rigidbody = rigidbody;
        _healthSystem = healthSystem;
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
        _healthSystem.onDie += PlayerDeath;
    }

    private void Unsubscribe()
    {
        _healthSystem.onDie -= PlayerDeath;
    }

    private void Update()
    {
        PlayMove();
    }

    private void PlayMove()
    {
        var velocity = _rigidbody.velocity;
        if (Mathf.Abs(velocity.x) >= _velocityTreshhold || 
            Mathf.Abs(velocity.z) >= _velocityTreshhold)
        {
            _animator.SetBool("isRunning", true);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    private void PlayerDeath()
    {
        _animator.SetTrigger("deathTrigger");
    }

    public void ResetState()
    {
        _animator.Play("PistolIdle");
    }
}
