using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class EnemyWeaponSystem : MonoBehaviour
{
    private EnemyStatsSystem _statsSystem;
    private ObjectPool _objectPool;

    private List<Weapon> _weapons;
    private Weapon _currentWeapon;
    private Queue<Coroutine> _reloadCoroutines = new Queue<Coroutine>();
    private bool _isInitializated = false;
    
    public void Init(EnemyStatsSystem statsSystem, ObjectPool objectPool)
    {
        _statsSystem = statsSystem;
        _objectPool = objectPool;
        _isInitializated = true;
        CacheComponents();
        Subscribe();
        ResetWeapons();
        InvokeRepeating("ShootHandler", 0, 1);
    }

    private void OnEnable()
    {
        if (_isInitializated)
        {
            Subscribe();
            ResetWeapons();
            InvokeRepeating("ShootHandler", 0, 1);
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
        CancelInvoke("ShootHandler");
    }

    public void ResetWeapons()
    {
        if (_isInitializated)
        {
            SortWeapons();
            StopAllReloads();
            InitWeapons();
            EquipWeapon(0);
        }
    }

    private void CacheComponents()
    {
        _weapons = new List<Weapon>(GetComponentsInChildren<Weapon>(true));
    }

    private void Subscribe()
    {
        _statsSystem.onStatsChanged += SetLevelUpMultipliers;
    }

    private void Unsubscribe()
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

    private IEnumerator Reload(Weapon weapon)
    {
        var reloadTime = weapon.GetReloadTime();
        var passedTime = 0f;
        
        while (passedTime <= reloadTime)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }

        weapon.FinishReload();
        _reloadCoroutines.Dequeue();
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
    }
}
