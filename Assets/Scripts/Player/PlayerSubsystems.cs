using System;
using UnityEngine;
using Structs;
using Enums;

public class PlayerSubsystems : MonoBehaviour, ISaveable
{
    private PlayerHealthSystem _healthSystem;
    private PlayerWeaponSystem _weaponSystem;
    private PlayerStatsSystem _statsSystem;
    private PlayerExperienceSystem _experienceSystem;
    private PlayerInputSystem _inputSystem;

    public PlayerInputSystem InputSystem => _inputSystem;

    public event Action<int> onAmmoChanged;
    public event Action<EWeaponType> onWeaponChanged;
    public event Action<float> onExperienceProgressChanged;
    public event Action<int> onLevelChanged;
    public event Action<SPlayerStatsMultipliers> onStatsChanged;

    public void Init(
        PlayerExperienceSystem experienceSystem, 
        PlayerStatsSystem statsSystem,
        PlayerWeaponSystem weaponSystem, 
        PlayerHealthSystem healthSystem,
        PlayerInputSystem inputSystem) 
    {
        _experienceSystem = experienceSystem;
        _statsSystem = statsSystem;
        _weaponSystem = weaponSystem;
        _healthSystem = healthSystem;
        _inputSystem = inputSystem;

        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        //_healthSystem.OnDie += Die;
        _weaponSystem.onAmmoChanged += ammo => onAmmoChanged?.Invoke(ammo);
        _weaponSystem.onWeaponChanged += weaponName => onWeaponChanged?.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged += progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged += level => onLevelChanged?.Invoke(level);
        _statsSystem.onStatsChanged += statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
    }

    private void Unsubscribe()
    {
        //_healthSystem.OnDie -= Die;
        _weaponSystem.onAmmoChanged -= ammo => onAmmoChanged.Invoke(ammo);
        _weaponSystem.onWeaponChanged -= weaponName => onWeaponChanged.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged -= progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged -= level => onLevelChanged(level);
        _statsSystem.onStatsChanged -= statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
    }

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
