using UnityEngine;

public class PlayerMovementSystem : MonoBehaviour
{
    private float _baseMoveSpeed = 5;
    private float _moveSpeed;
    private bool _mustMove = true;
    private PlayerStatsSystem _statsSystem;
    private PlayerInputSystem _inputSystem;
    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        _moveSpeed = _baseMoveSpeed;
    }

    private void OnEnable()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
        _inputSystem.onDirectionAxisPressed += SetMoveDirection;
    }

    private void OnDisable()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void FixedUpdate()
    {
        MoveIfNecessary();
    }

    private void CacheComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<PlayerStatsSystem>();
        _inputSystem = GetComponent<PlayerInputSystem>();
    }

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

    private void ApplyLevelUpMultipliers(PlayerStatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }
}
