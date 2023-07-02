using UnityEngine;

public class Player : MonoBehaviour, ISaveable
{
    private PlayerHealthSystem _healthSystem;

    private void Awake()
    {
        CacheComponents();
    }

    //private void FixedUpdate()
    //{
    //    ActPhisicallyIfGameRunning();
    //}

    private void OnEnable()
    {
        _healthSystem.OnDie += Die;
    }

    private void OnDisable()
    {
        _healthSystem.OnDie += Die;
    }

    private void CacheComponents()
    {
        //_camera = Camera.main;
        //_rigidbody = GetComponent<Rigidbody>();
        //_statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<PlayerHealthSystem>();
        //_experienceSystem = GetComponent<ExperienceSystem>();
    }



    private void Die()
    {
        GameManager.Instance.GameOver();
    }

    //private void ActPhisicallyIfGameRunning()
    //{
    //    if (ShouldAct())
    //    {
    //        MovePlayer();
    //        RotatePlayer();
    //    }
    //    else
    //    {
    //        ResetVelosity();
    //    }
    //}



    //private void MovePlayer()
    //{
    //    _movement.x = Input.GetAxisRaw("Horizontal");
    //    _movement.z = Input.GetAxisRaw("Vertical");
    //    _movement.Normalize();
    //    _rigidbody.MovePosition(_rigidbody.position + _movement * _statsSystem.MoveSpeed * Time.deltaTime);
    //}

    //private void RotatePlayer()
    //{
    //    var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
    //    var playerToMouseDirection = mousePosition - transform.position;
    //    var angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
    //    Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
    //    _rigidbody.MoveRotation(targetRotation);
    //}

    //private void ResetVelosity()
    //{
    //    _rigidbody.velocity = Vector3.zero;
    //    _rigidbody.angularVelocity = Vector3.zero;
    //}



    //public void TakeDamage(float damage)
    //{
    //    _healthSystem.TakeDamage(damage);
    //}

    public EntityData CaptureState()
    {
        EntityData data = new EntityData();
        
        data.position = transform.position;
        //data.level = _experienceSystem.Level;
        //data.experience = _experienceSystem.Experience;
        //data.health = _statsSystem.CurrentHealth;
        //data.equippedWeaponIndex = _equippedWeaponIndex;
        //data.ammo = _currentWeapon.CurrentAmmo;
        return data;
    }

    public void LoadState(EntityData data)
    {
        transform.position = data.position;
        //_experienceSystem.Level = data.level;
        //_experienceSystem.AddExperience(data.experience);
        //EquipWeapon(data.equippedWeaponIndex);
        //_statsSystem.CurrentHealth = data.health;
        //_currentWeapon.CurrentAmmo = data.ammo;
    }
}
