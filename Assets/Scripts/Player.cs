using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : HealthSystem
{
    [SerializeField] private List<Weapon> _weapons = new List<Weapon>();
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rigidbody;

    private Vector3 _movement;
    private Camera _camera;
    private Weapon _currentWeapon;
    private int _equippedWeaponIndex;

    private void Start()
    {
        CacheComponents();
        InitHealth();
        InitializeWeapon();
    }

    private void CacheComponents()
    {
        _camera = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (IsDead || !GameManager.Instance.IsStarted || GameManager.Instance.IsPaused)
        {
            ResetVelosity();
            return;
        }

        CheckIfKilled();
        Regenerate();
        MovePlayer();
        RotatePlayer();
        ShootHandler();
        ReloadHandler();
        ScrollWeaponSelect();
    }

    private void CheckIfKilled()
    {
        if (_health <= 0)
        {
            IsDead = true;
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    private void MovePlayer()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        transform.position += _movement * _moveSpeed * Time.deltaTime;
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

    private void InitializeWeapon()
    {
        _currentWeapon = _weapons[0];
        _equippedWeaponIndex = 0;
        UIManager.Instance.SetWeapon(_currentWeapon.gameObject.name);
        
        for (int i = 1; i < _weapons.Count; i++)
        {
            _weapons[i].gameObject.SetActive(false);
        }
    }

    private void ShootHandler()
    {
        if (Input.GetMouseButton(0))
        {
            _currentWeapon.Shoot();
        }
    }

    private void ReloadHandler()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentWeapon.BeginReload();
            StartCoroutine(Reload(_currentWeapon));
        }
    }

    private IEnumerator Reload(Weapon weapon)
    {
        yield return new WaitForSeconds(weapon.GetReloadTime());
        weapon.FinishReload();
    }

    private void ScrollWeaponSelect()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            Debug.Log("Equip previous weapon");
            EquipPreviousWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Equip next weapon");
            EquipNextWeapon();
        }
    }

    private void EquipNextWeapon()
    {
        _equippedWeaponIndex = (_equippedWeaponIndex + 1) % _weapons.Count;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _currentWeapon = _weapons[i];
                _currentWeapon.gameObject.SetActive(true);
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }

        UIManager.Instance.SetWeapon(_currentWeapon.gameObject.name);
    }

    private void EquipPreviousWeapon()
    {
        _equippedWeaponIndex = _equippedWeaponIndex ==  0 ? _weapons.Count - 1 : --_equippedWeaponIndex;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _currentWeapon = _weapons[i];
                _currentWeapon.gameObject.SetActive(true);
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }

        UIManager.Instance.SetWeapon(_currentWeapon.gameObject.name);
    }
}
