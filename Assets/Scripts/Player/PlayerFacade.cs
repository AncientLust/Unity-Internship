using System;
using UnityEngine;
using Structs;
using Enums;

public class PlayerFacade : MonoBehaviour, ISaveable
{
    private PlayerHealthSystem _healthSystem; // Must be injected
    private PlayerWeaponSystem _weaponSystem; // Must be injected
    private PlayerExperienceSystem _experienceSystem; // Must be injected
    private PlayerStatsSystem _statsSystem; // Must be injected

    public event Action<int> onAmmoChanged; 
    public event Action<EWeaponType> onWeaponChanged;
    public event Action<float> onExperienceProgressChanged;
    public event Action<int> onLevelChanged;
    public event Action<SPlayerStatsMultipliers> onStatsChanged;

    public void Init(
        PlayerExperienceSystem experienceSystem, 
        PlayerStatsSystem statsSystem,
        PlayerWeaponSystem weaponSystem, 
        PlayerHealthSystem healthSystem) 
    {
        _experienceSystem = experienceSystem;
        _statsSystem = statsSystem;
        _weaponSystem = weaponSystem;
        _healthSystem = healthSystem;

        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _healthSystem.OnDie += Die;
        _weaponSystem.onAmmoChanged += ammo => onAmmoChanged?.Invoke(ammo);
        _weaponSystem.onWeaponChanged += weaponName => onWeaponChanged?.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged += progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged += level => onLevelChanged?.Invoke(level);
        _statsSystem.onStatsChanged += statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
    }

    private void UnsubscribeEvents()
    {
        _healthSystem.OnDie -= Die;
        _weaponSystem.onAmmoChanged -= ammo => onAmmoChanged.Invoke(ammo);
        _weaponSystem.onWeaponChanged -= weaponName => onWeaponChanged.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged -= progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged -= level => onLevelChanged(level);
        _statsSystem.onStatsChanged -= statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
    }

    //private void CacheComponents()
    //{
    //    _healthSystem = GetComponent<PlayerHealthSystem>();
    //    _weaponSystem = GetComponent<PlayerWeaponSystem>();
    //    _experienceSystem = GetComponent<PlayerExperienceSystem>();
    //    _statsSystem = GetComponent<PlayerStatsSystem>();
    //}

    private void Die()
    {
        //GameManager.Instance.GameOver();
    }

    public EntityData CaptureState()
    {
        EntityData data = new EntityData();
        
        //data.position = transform.position;
        //data.level = _experienceSystem.Level;
        //data.experience = _experienceSystem.Experience;
        //data.health = _statsSystem.CurrentHealth;
        //data.equippedWeaponIndex = _equippedWeaponIndex;
        //data.ammo = _currentWeapon.CurrentAmmo;
        return data;
    }

    public void LoadState(EntityData data)
    {
        //transform.position = data.position;
        //_experienceSystem.Level = data.level;
        //_experienceSystem.AddExperience(data.experience);
        //EquipWeapon(data.equippedWeaponIndex);
        //_statsSystem.CurrentHealth = data.health;
        //_currentWeapon.CurrentAmmo = data.ammo;
    }
}
