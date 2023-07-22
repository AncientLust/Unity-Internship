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
    private PlayerSaveLoadSystem _saveLoadSystem;
    private PlayerEffectsSystem _effectsSystem;
    private PlayerSkillSystem _skillSystem;

    public event Action<int> onAmmoChanged;
    public event Action<EWeaponType> onWeaponChanged;
    public event Action<float> onReloadProgressChanged;
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
        PlayerMovementSystem movementSystem,
        PlayerSaveLoadSystem saveLoadSystem,
        PlayerEffectsSystem effectsSystem,
        PlayerSkillSystem skillSystem)
    {
        _experienceSystem = experienceSystem;
        _statsSystem = statsSystem;
        _weaponSystem = weaponSystem;
        _healthSystem = healthSystem;
        _inputSystem = inputSystem;
        _movementSystem = movementSystem;
        _saveLoadSystem = saveLoadSystem;
        _effectsSystem = effectsSystem;
        _skillSystem = skillSystem;

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
        _weaponSystem.onReloadProgressChanged += reloadProgress => onReloadProgressChanged.Invoke(reloadProgress);
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
        _skillSystem.ResetSkillsCooldown();
        _effectsSystem.StopAllEffects();

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

    public void SaveState()
    {
        _saveLoadSystem.Save();
    }

    public void LoadState()
    {
        _saveLoadSystem.Load();
    }
}
