using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();

    private Rigidbody _rigidbody;
    private StatsSystem _statsSystem;
    private HealthSystem _healthSystem;
    private int _equippedWeaponIndex;
    private Vector3 _movement;
    private Camera _camera;
    private Weapon _currentWeapon;

    private void Start()
    {
        CacheComponents();
        InitializeWeapon();
    }

    private void Update()
    {
        ActIfGameRunning();
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        _statsSystem = GetComponent<StatsSystem>();
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void ActIfGameRunning()
    {
        if (_healthSystem.IsDead || !GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            ResetVelosity();
            return;
        }

        _healthSystem.Regenerate();
        _healthSystem.HideHealthIfHealthy();

        MovePlayer();
        RotatePlayer();
        ShootHandler();
        ReloadHandler();
        ScrollWeaponSelect();
    }

    private void MovePlayer()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        transform.position += _movement * _statsSystem.MoveSpeed * Time.deltaTime;
    }

    private void RotatePlayer()
    {
        var mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        var playerToMouseDirection = mousePosition - transform.position;
        var angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void ResetVelosity()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void ShootHandler()
    {
        if (Input.GetMouseButton(0))
        {
            if (!_currentWeapon.HasEmptyClip())
            {
                _currentWeapon.Shoot();
            }
            else
            {
                Reload();
            }
        }
    }

    private void ReloadHandler()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        _currentWeapon.ClipCapacityMultiplier = _statsSystem.AmmoMultiplier;
        _currentWeapon.BeginReload();
        StartCoroutine(Reload(_currentWeapon));
    }

    private IEnumerator Reload(Weapon weapon)
    {
        yield return new WaitForSeconds(weapon.GetReloadTime() * _statsSystem.ReloadMultiplier);
        weapon.FinishReload();
    }

    private void InitializeWeapon()
    {
        _equippedWeaponIndex = 0;
        _currentWeapon = _weapons[0];
        _currentWeapon.PowerMultiplier = _statsSystem.PowerMultiplier;
        _currentWeapon.ClipCapacityMultiplier = _statsSystem.AmmoMultiplier;
        GameplayUI.Instance.SetWeapon(_currentWeapon.gameObject.name);

        for (int i = 1; i < _weapons.Count; i++)
        {
            _weapons[i].gameObject.SetActive(false);
        }
    }

    private void ScrollWeaponSelect()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            EquipPreviousWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            EquipNextWeapon();
        }
    }

    private void EquipNextWeapon()
    {
        Debug.Log("Equip next weapon");
        EquipWeapon((_equippedWeaponIndex + 1) % _weapons.Count);
    }

    private void EquipPreviousWeapon()
    {
        Debug.Log("Equip previous weapon");
        EquipWeapon(_equippedWeaponIndex == 0 ? _weapons.Count - 1 : --_equippedWeaponIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        _equippedWeaponIndex = weaponIndex;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _currentWeapon = _weapons[i];
                _currentWeapon.PowerMultiplier = _statsSystem.PowerMultiplier;
                _currentWeapon.ClipCapacityMultiplier = _statsSystem.AmmoMultiplier;
                _currentWeapon.gameObject.SetActive(true);
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }

        GameplayUI.Instance.SetWeapon(_currentWeapon.gameObject.name);
    }
}
