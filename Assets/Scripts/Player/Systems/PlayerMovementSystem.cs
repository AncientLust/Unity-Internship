using UnityEngine;
using Structs;

public class PlayerMovementSystem : MonoBehaviour
{
    private float _baseMoveSpeed = 4;
    private float _moveSpeed;
    private Vector3 _moveDirection;
    private bool _isActive;
    private PlayerStatsSystem _statsSystem;
    private PlayerInputSystem _inputSystem;
    private PlayerHealthSystem _healthSystem;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    public void Init(
        PlayerStatsSystem statsSystem, 
        PlayerInputSystem inputSystem, 
        Rigidbody rigidbody, 
        PlayerHealthSystem healthSystem,
        CapsuleCollider collider)
    {
        _statsSystem = statsSystem;
        _inputSystem = inputSystem;
        _rigidbody = rigidbody;
        _collider = collider;
        _healthSystem = healthSystem;
        _isActive = false;
        Subscribe();
    }

    private void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed += SetMoveDirection;
        _healthSystem.onDie += () => SetActive(false);
    }

    private void Unsubscribe()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed -= SetMoveDirection;
        _healthSystem.onDie -= () => SetActive(false);
    }

    private void Update()
    {
        MoveIfNecessary();
    }

    private void MoveIfNecessary()
    {
        if (_isActive)
        {
            Move();
            Rotate();
        }
    }

    private void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    private void Move()
    {
        _moveDirection.Normalize();
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
        _moveDirection = Vector3.zero;
    }

    private void Rotate()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var playerToMouseDirection = mousePosition - transform.position;
        var angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = targetRotation;
    }

    public void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void ApplyLevelUpMultipliers(SPlayerStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }

    public void Push(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void SetActive(bool state)
    {
        _rigidbody.isKinematic = !state;
        _collider.enabled = state;
        _isActive = state;
    }
}
