using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;

public class PlayerWeaponSystem : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem; // Must be injected
    private PlayerStatsSystem _statsSystem; // Must be injected
    private List<Weapon> _weapons = new List<Weapon>();
    private Weapon _currentWeapon;
    private int _equippedWeaponIndex;

    public Action<EWeaponType> onWeaponChanged;
    public Action<int> onAmmoChanged;

    public void Init(PlayerInputSystem playerInputSystem, PlayerStatsSystem playerStatsSystem)
    {
        _playerInputSystem = playerInputSystem;
        _statsSystem = playerStatsSystem;
        SubscribeEvents();
    }

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        SortWeapons();
        EquipWeapon(0); 
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void CacheComponents()
    {
        _weapons = new List<Weapon>(GetComponentsInChildren<Weapon>(true));
        //_playerInputSystem = GetComponent<PlayerInputSystem>();
        //_statsSystem = GetComponent<PlayerStatsSystem>();
    }

    private void SubscribeEvents()
    {
        _playerInputSystem.onScrollUp += EquipNextWeapon;
        _playerInputSystem.onScrollDown += EquipPreviousWeapon;
        _playerInputSystem.onLeftMouseClicked += ShootHandler;
        _playerInputSystem.onReloadPressed += ReloadHandler;
        _statsSystem.onStatsChanged += SetLevelUpMultipliers;
    }

    private void UnsubscribeEvents()
    {
        _playerInputSystem.onScrollUp -= EquipNextWeapon;
        _playerInputSystem.onScrollDown -= EquipPreviousWeapon;
        _playerInputSystem.onLeftMouseClicked -= ShootHandler;
        _playerInputSystem.onReloadPressed -= ReloadHandler;
        _statsSystem.onStatsChanged -= SetLevelUpMultipliers;
    }

    private void SetLevelUpMultipliers(SPlayerStatsMultipliers multipliers)
    {
        foreach (var weapon in _weapons)
        {
            weapon.DamageMultiplier = multipliers.damage;
            weapon.AmmoMultiplier = multipliers.ammo;
            weapon.ReloadMultiplier = multipliers.reload;
        }
    }

    private void ShootHandler()
    {
        if (!_currentWeapon.HasEmptyClip())
        {
            _currentWeapon.Shoot();
            onAmmoChanged.Invoke(_currentWeapon.Ammo);
        }
        else
        {
            ReloadHandler();
        }
    }

    private void ReloadHandler()
    {
        if (!_currentWeapon.InReloading)
        {
            _currentWeapon.BeginReload();
            StartCoroutine(Reload(_currentWeapon));
        }
    }

    private IEnumerator Reload(Weapon weapon)
    {
        yield return new WaitForSeconds(weapon.GetReloadTime());
        weapon.FinishReload();

        if (weapon == _currentWeapon)
        {
            onAmmoChanged.Invoke(_currentWeapon.Ammo);
        }
    }

    private void EquipNextWeapon()
    {
        EquipWeapon((_equippedWeaponIndex + 1) % _weapons.Count);
    }

    private void EquipPreviousWeapon()
    {
        EquipWeapon(_equippedWeaponIndex == 0 ? _weapons.Count - 1 : --_equippedWeaponIndex);
    }

    private void SortWeapons()
    {
        _weapons.Sort((weapon1, weapon2) => weapon1.Type.CompareTo(weapon2.Type));
    }

    private void EquipWeapon(int weaponIndex)
    {
        _equippedWeaponIndex = weaponIndex;

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

        onWeaponChanged.Invoke(_currentWeapon.Type);
        onAmmoChanged.Invoke(_currentWeapon.Ammo);
    }
}
