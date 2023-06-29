using UnityEngine;

public class Player : MonoBehaviour, IDamageable, ISaveable
{
    private Rigidbody _rigidbody;
    private StatsSystem _statsSystem;
    private HealthSystem _healthSystem;
    private ExperienceSystem _experienceSystem;
    private Vector3 _movement;
    private Camera _camera;

    public delegate void OnScrollUpHandler();
    public delegate void OnScrollDownHandler();
    public delegate void OnLeftMouseClickedHandler();
    public delegate void OnReloadPressedHandler();

    public event OnScrollUpHandler onScrollUp;
    public event OnScrollDownHandler onScrollDown;
    public event OnLeftMouseClickedHandler onLeftMouseClicked;
    public event OnReloadPressedHandler onReloadPressed;

    private void Start()
    {
        CacheComponents();
    }

    private void Update()
    {
        ActIfGameRunning();
    }

    private void FixedUpdate()
    {
        ActPhisicallyIfGameRunning();
    }

    public void Die()
    {
        GameManager.Instance.GameOver();
        gameObject.SetActive(false);
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
        _experienceSystem = GetComponent<ExperienceSystem>();
    }

    private void ActIfGameRunning()
    {
        if (ShouldAct())
        {
            _healthSystem.Regenerate();
            InputHandler();
        }
    }

    private void ActPhisicallyIfGameRunning()
    {
        if (ShouldAct())
        {
            MovePlayer();
            RotatePlayer();
        }
        else
        {
            ResetVelosity();
        }
    }

    private bool ShouldAct()
    {
        return !_healthSystem.IsDead && GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    }

    private void MovePlayer()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        _movement.Normalize();
        _rigidbody.MovePosition(_rigidbody.position + _movement * _statsSystem.MoveSpeed * Time.deltaTime);
    }

    private void RotatePlayer()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
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

    private void InputHandler()
    {
        if (Input.GetMouseButton(0))
        {
            onLeftMouseClicked.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            onReloadPressed.Invoke();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            onScrollDown.Invoke();
        }
        
        if (Input.mouseScrollDelta.y < 0)
        {
            onScrollUp.Invoke();
        }
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.TakeDamage(damage);
    }

    public EntityData CaptureState()
    {
        EntityData data = new EntityData();
        
        data.position = transform.position;
        data.level = _experienceSystem.Level;
        data.experience = _experienceSystem.Experience;
        data.health = _statsSystem.CurrentHealth;
        //data.equippedWeaponIndex = _equippedWeaponIndex;
        //data.ammo = _currentWeapon.CurrentAmmo;
        return data;
    }

    public void LoadState(EntityData data)
    {
        transform.position = data.position;
        _experienceSystem.Level = data.level;
        _experienceSystem.AddExperience(data.experience);
        //EquipWeapon(data.equippedWeaponIndex);
        _statsSystem.CurrentHealth = data.health;
        //_currentWeapon.CurrentAmmo = data.ammo;
    }
}
