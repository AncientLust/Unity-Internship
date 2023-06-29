using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private float _baseMoveSpeed;
    private float _moveSpeed;
    private bool _mustMove = true;
    private StatsSystem _statsSystem;
    private Rigidbody _rigidbody;
    private Vector3 _moveVector;

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
        _statsSystem = GetComponent<StatsSystem>();
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

    private void Move()
    {
        _moveVector.x = Input.GetAxisRaw("Horizontal");
        _moveVector.z = Input.GetAxisRaw("Vertical");
        _moveVector.Normalize();
        _rigidbody.MovePosition(_rigidbody.position + _moveVector * _moveSpeed * Time.deltaTime);
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

    private void ApplyLevelUpMultipliers(StatsMultipliers multipliers)
    {
        _moveSpeed = _baseMoveSpeed * multipliers.moveSpeed;
    }
}
