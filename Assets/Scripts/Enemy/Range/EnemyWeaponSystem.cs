using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Structs;

public class EnemyWeaponSystem : MonoBehaviour
{
    private float _firstShootDelay = 2;
    private float _shootRepeatTime = 2f;
    private EnemyStatsSystem _statsSystem;
    private ObjectPool _objectPool;
    private EnemyHealthSystem _healthSystem;
    private List<Weapon> _weapons;
    private Weapon _currentWeapon;
    private Queue<Coroutine> _reloadCoroutines = new Queue<Coroutine>();
    private IAudioPlayer _iAudioPlayer;
    private bool _isInitializated = false;
    
    public void Init(EnemyStatsSystem statsSystem, ObjectPool objectPool, EnemyHealthSystem healthSystem, IAudioPlayer iAudioPlayer)
    {
        _statsSystem = statsSystem;
        _objectPool = objectPool;
        _healthSystem = healthSystem;
        _iAudioPlayer = iAudioPlayer;
        _isInitializated = true;
        
        CacheComponents();
        Subscribe();
        ResetWeapons();
        InvokeRepeating("ShootHandler", _firstShootDelay, _shootRepeatTime);
    }

    private void OnEnable()
    {
        if (_isInitializated)
        {
            Subscribe();
            ResetWeapons();
            InvokeRepeating("ShootHandler", _firstShootDelay, _shootRepeatTime);
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
        CancelInvoke("ShootHandler");
    }

    public void ResetWeapons()
    {
        SortWeapons();
        StopAllReloads();
        InitWeapons();
        EquipWeapon(0);
    }

    private void CacheComponents()
    {
        _weapons = new List<Weapon>(GetComponentsInChildren<Weapon>(true));
    }

    private void Subscribe()
    {
        _statsSystem.onStatsChanged += SetLevelUpMultipliers;
        _healthSystem.onDie += () => CancelInvoke("ShootHandler");
    }

    private void Unsubscribe()
    {
        _statsSystem.onStatsChanged -= SetLevelUpMultipliers;
        _healthSystem.onDie -= () => CancelInvoke("ShootHandler");
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
        if (!_currentWeapon.HasEmptyClip() &&
            !_currentWeapon.IsInDowntime() &&
            !_currentWeapon.InReloading)
        {
            _currentWeapon.Shoot(); 
            _iAudioPlayer.PlaySound(_currentWeapon.ShootSound);
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
