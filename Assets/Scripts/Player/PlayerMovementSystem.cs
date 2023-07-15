using UnityEngine;
using Structs;

public class PlayerMovementSystem : MonoBehaviour, ITargetable
{
    private float _baseMoveSpeed = 5;
    private float _moveSpeed;
    private bool _mustMove = true;
    private Vector3 _moveDirection;
    private PlayerStatsSystem _statsSystem;
    private PlayerInputSystem _inputSystem;
    private Rigidbody _rigidbody;

    public Transform Transform => transform;

    public void Init(PlayerStatsSystem statsSystem, PlayerInputSystem inputSystem, Rigidbody rigidbody)
    {
        _statsSystem = statsSystem;
        _inputSystem = inputSystem;
        _rigidbody = rigidbody;

        SubscribeEvents();
    }

    private void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed += SetMoveDirection;
    }

    private void UnsubscribeEvents()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed -= SetMoveDirection;
    }

    private void FixedUpdate()
    {
        MoveIfNecessary();
    }

    //private void CacheComponents()
    //{
    //    _rigidbody = GetComponent<Rigidbody>();
    //    _statsSystem = GetComponent<PlayerStatsSystem>();
    //    _inputSystem = GetComponent<PlayerInputSystem>();
    //}

    private void MoveIfNecessary()
    {
        if (_mustMove)
        {
            Move();
            Rotate();
        }
        else
        {
            ResetVelosity();
        }
    }

    private void SetMoveDirection(Vector3 direction)
    {
        _moveDirection = direction;
    }

    private void Move()
    {
        if (_mustMove)
        {
            _moveDirection.Normalize();
            _rigidbody.MovePosition(_rigidbody.position + _moveDirection * _moveSpeed * Time.deltaTime);
            _moveDirection = Vector3.zero;
        }
    }

    private void Rotate()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var playerToMouseDirection = mousePosition - transform.position;
        var angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
        
        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
        _rigidbody.MoveRotation(targetRotation);
    }

    private void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void ApplyLevelUpMultipliers(SPlayerStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }
}
