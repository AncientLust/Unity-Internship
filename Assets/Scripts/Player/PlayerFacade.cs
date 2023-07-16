using System;
using UnityEngine;
using Structs;
using Enums;

public class PlayerFacade : MonoBehaviour, 
    IPlayerFacade, IHUDCompatible, IExperienceTaker, IDamageable, ITargetable
{
    private PlayerHealthSystem _healthSystem;
    private PlayerWeaponSystem _weaponSystem;
    private PlayerStatsSystem _statsSystem;
    private PlayerExperienceSystem _experienceSystem;
    private PlayerInputSystem _inputSystem;
    private PlayerMovementSystem _movementSystem;

    public event Action<int> onAmmoChanged;
    public event Action<EWeaponType> onWeaponChanged;
    public event Action<float> onExperienceProgressChanged;
    public event Action<int> onLevelChanged;
    public event Action<SPlayerStatsMultipliers> onStatsChanged;
    public event Action onDie;

    public Transform Transform => transform;

    public void Init(
        PlayerExperienceSystem experienceSystem,
        PlayerStatsSystem statsSystem,
        PlayerWeaponSystem weaponSystem,
        PlayerHealthSystem healthSystem,
        PlayerInputSystem inputSystem,
        PlayerMovementSystem movementSystem)
    {
        _experienceSystem = experienceSystem;
        _statsSystem = statsSystem;
        _weaponSystem = weaponSystem;
        _healthSystem = healthSystem;
        _inputSystem = inputSystem;
        _movementSystem = movementSystem;

        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _weaponSystem.onAmmoChanged += ammo => onAmmoChanged.Invoke(ammo);
        _weaponSystem.onWeaponChanged += weaponName => onWeaponChanged.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged += progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged += level => onLevelChanged.Invoke(level);
        _statsSystem.onStatsChanged += statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
        _healthSystem.onDie += () => onDie.Invoke();
    }

    private void Unsubscribe()
    {
        _weaponSystem.onAmmoChanged -= ammo => onAmmoChanged.Invoke(ammo);
        _weaponSystem.onWeaponChanged -= weaponName => onWeaponChanged.Invoke(weaponName);
        _experienceSystem.onExperienceProgressChanged -= progress => onExperienceProgressChanged.Invoke(progress);
        _experienceSystem.onLevelChanged -= level => onLevelChanged(level);
        _statsSystem.onStatsChanged -= statsMultipliers => onStatsChanged.Invoke(statsMultipliers);
    }

    public void TakeExperience(float experience)
    {
        _experienceSystem.AddExperience(experience);
    }

    public void EnableForGameSession()
    {
        _experienceSystem.SetLevel(1);
        _healthSystem.ResetHealth();
        _movementSystem.ResetPosition();
        _weaponSystem.ResetWeapons();

        _movementSystem.IsActive = true;
        _inputSystem.IsActive = true;
    }

    public void DisableForGameSession()
    {
        SetInputHandling(false);
    }

    public void TakeDamage(float damage)
    {
        _healthSystem.ReduceHealth(damage);
    }

    public void SetInputHandling(bool state)
    {
        _movementSystem.IsActive = state;
        _inputSystem.IsActive = state;
    }

    //public EntityData CaptureState()
    //{
    //    EntityData data = new EntityData();

    //    //data.position = transform.position;
    //    //data.level = _experienceSystem.Level;
    //    //data.experience = _experienceSystem.Experience;
    //    //data.health = _statsSystem.CurrentHealth;
    //    //data.equippedWeaponIndex = _equippedWeaponIndex;
    //    //data.ammo = _currentWeapon.CurrentAmmo;
    //    return data;
    //}

    //public void LoadState(EntityData data)
    //{
    //    //transform.position = data.position;
    //    //_experienceSystem.Level = data.level;
    //    //_experienceSystem.AddExperience(data.experience);
    //    //EquipWeapon(data.equippedWeaponIndex);
    //    //_statsSystem.CurrentHealth = data.health;
    //    //_currentWeapon.CurrentAmmo = data.ammo;
    //}


}
