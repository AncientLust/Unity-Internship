using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] List<Weapon> _weapons = new List<Weapon>();

    Vector3 _movement;
    Camera _camera;
    Weapon _currentWeapon;
    int _equippedWeaponIndex;

    private void Start()
    {
        _camera = Camera.main;
        InitializeWeapon();
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
        ShootHandler();
        ReloadHandler();
        ScrollWeaponSelect();
    }

    void MovePlayer()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.z = Input.GetAxisRaw("Vertical");
        transform.position += _movement * _moveSpeed * Time.deltaTime;
    }

    void RotatePlayer()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerToMouseDirection = mousePosition - transform.position;
        float angle = Mathf.Atan2(playerToMouseDirection.x, playerToMouseDirection.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void InitializeWeapon()
    {
        _currentWeapon = _weapons[0];
        _equippedWeaponIndex = 0;
        for (int i = 1; i < _weapons.Count; i++)
        {
            _weapons[i].gameObject.SetActive(false);
        }
    }

    private void ShootHandler()
    {
        if (Input.GetMouseButtonDown(0))
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
            Debug.Log("Equip next weapon");
            EquipNextWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            Debug.Log("Equip previous weapon");
            EquipPreviousWeapon();
        }
    }

    private void EquipNextWeapon()
    {
        _equippedWeaponIndex = (_equippedWeaponIndex + 1) % _weapons.Count;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _weapons[i].gameObject.SetActive(true);
                _currentWeapon = _weapons[i];
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }
    }

    private void EquipPreviousWeapon()
    {
        _equippedWeaponIndex = _equippedWeaponIndex ==  0 ? _weapons.Count - 1 : --_equippedWeaponIndex;

        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == _equippedWeaponIndex)
            {
                _weapons[i].gameObject.SetActive(true);
                _currentWeapon = _weapons[i];
                continue;
            }

            _weapons[i].gameObject.SetActive(false);
        }
    }
}
