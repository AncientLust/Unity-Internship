using UnityEngine;
using Structs;

public class PlayerMovementSystem : MonoBehaviour
{
    protected float _baseMoveSpeed = 5;
    protected float _moveSpeed;
    protected Vector3 _moveDirection;
    protected PlayerStatsSystem _statsSystem;
    protected PlayerInputSystem _inputSystem;
    protected Rigidbody _rigidbody;

    public bool IsActive { get; set; } = false;

    public void Init(PlayerStatsSystem statsSystem, PlayerInputSystem inputSystem, Rigidbody rigidbody)
    {
        _statsSystem = statsSystem;
        _inputSystem = inputSystem;
        _rigidbody = rigidbody;

        SubscribeEvents();
    }

    protected void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }

    protected void OnDisable()
    {
        UnsubscribeEvents();
    }

    protected void SubscribeEvents()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed += SetMoveDirection;
    }

    protected void UnsubscribeEvents()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed -= SetMoveDirection;
    }

    protected void FixedUpdate()
    {
        MoveIfNecessary();
    }

    protected void MoveIfNecessary()
    {
        if (IsActive)
        {
            Move();
            Rotate();
        }
        else
        {
            ResetVelosity();
        }
    }

    protected void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    protected void Move()
    {
        _moveDirection.Normalize();
        _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.deltaTime);
        _moveDirection = Vector3.zero;
    }

    protected void Rotate()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var playerToMouseDirection = mousePosition - transform.position;
        var angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        _rigidbody.MoveRotation(targetRotation);
    }

    protected void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    protected void ApplyLevelUpMultipliers(SPlayerStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
    }
}
