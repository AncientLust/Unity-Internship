using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;

public class PlayerWeaponSystem : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;
    private PlayerStatsSystem _statsSystem;
    private ObjectPool _objectPool;
    
    private List<IWeapon> _weapons;
    private IWeapon _currentWeapon;
    private int _equippedWeaponIndex;
    private Queue<Coroutine> _reloadCoroutines = new Queue<Coroutine>();

    public Action<EWeaponType> onWeaponChanged;
    public Action<int> onAmmoChanged;
    public Action<float> onReloadProgressChanged;
    
    public void Init(PlayerInputSystem inputSystem, PlayerStatsSystem statsSystem, ObjectPool objectPool)
    {
        _playerInputSystem = inputSystem;
        _statsSystem = statsSystem;
        _objectPool = objectPool;
        SubscribeEvents();
    }

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        ResetWeapons();
    }

    public void ResetWeapons()
    {
        SortWeapons();
        StopAllReloads();
        InitWeapons();
        EquipWeapon(0);
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void CacheComponents()
    {
        _weapons = new List<IWeapon>(GetComponentsInChildren<IWeapon>(true));
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
            var reloadSession = StartCoroutine(Reload(_currentWeapon));
            _reloadCoroutines.Enqueue(reloadSession);
        }
    }

    private IEnumerator Reload(IWeapon weapon)
    {
        var reloadTime = weapon.GetReloadTime();
        var passedTime = 0f;
        
        while (passedTime <= reloadTime)
        {
            if (weapon == _currentWeapon)
            {
                onReloadProgressChanged.Invoke(passedTime / reloadTime);
            }

            passedTime += Time.deltaTime;
            yield return null;
        }

        weapon.FinishReload();
        _reloadCoroutines.Dequeue();

        if (weapon == _currentWeapon)
        {
            onAmmoChanged.Invoke(_currentWeapon.Ammo);
        }
    }

    private void StopAllReloads()
    {
        foreach (var reload in _reloadCoroutines)
        {
            StopCoroutine(reload);
        }

        _reloadCoroutines.Clear();
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

    private void InitWeapons()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Init(_objectPool);
        }
    }

    private void EquipWeapon(int weaponIndex)
    {
        for (int i = 0; i < _weapons.Count; i++)
        {
            if (i == weaponIndex)
            {
                _currentWeapon = _weapons[i];
                _currentWeapon.SetPrefabState(true);
                continue;
            }

            _weapons[i].SetPrefabState(false);
        }

        _equippedWeaponIndex = weaponIndex;
        onWeaponChanged.Invoke(_currentWeapon.Type);
        onAmmoChanged.Invoke(_currentWeapon.Ammo);

        if (!_currentWeapon.InReloading)
        {
            onReloadProgressChanged(1f);
        }
    }
}
