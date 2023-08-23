using UnityEngine;

public class PlayerAnimationSystem : MonoBehaviour
{
    private Animator _animator;
    private PlayerHealthSystem _healthSystem;
    private float _velocityTreshhold = 1;
    private Vector3 _previousPosition;
    private Vector3 _estimatedVelocity;
    private bool _isInitialized;

    public void Init(PlayerHealthSystem healthSystem)
    {
        _animator = GetComponent<Animator>();
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
        CalculateEstimatedVelocity();
        PlayMove();
    }

    private void CalculateEstimatedVelocity()
    {
        _estimatedVelocity = (transform.position - _previousPosition) / Time.deltaTime;
        _previousPosition = transform.position;
    }

    private void PlayMove()
    {
        if (Mathf.Abs(_estimatedVelocity.x) >= _velocityTreshhold ||
            Mathf.Abs(_estimatedVelocity.z) >= _velocityTreshhold)
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
