using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Structs;

public class EnemyWeaponSystem : MonoBehaviour
{
    private EnemyStatsSystem _statsSystem;
    private ObjectPool _objectPool;
    
    private List<IWeapon> _weapons;
    private IWeapon _currentWeapon;
    private Queue<Coroutine> _reloadCoroutines = new Queue<Coroutine>();

    public Action<EWeaponType> onWeaponChanged;
    public Action<int> onAmmoChanged;
    public Action<float> onReloadProgressChanged;
    
    public void Init(EnemyStatsSystem statsSystem, ObjectPool objectPool)
    {
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
        _statsSystem.onStatsChanged += SetLevelUpMultipliers;
    }

    private void UnsubscribeEvents()
    {
        _statsSystem.onStatsChanged -= SetLevelUpMultipliers;
    }

    private void SetLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
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

        onWeaponChanged.Invoke(_currentWeapon.Type);
        onAmmoChanged.Invoke(_currentWeapon.Ammo);

        if (!_currentWeapon.InReloading)
        {
            onReloadProgressChanged(1f);
        }
    }
}
