using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Enums;
using Structs;

public class HUD : MonoBehaviour
{
    [SerializeField] private WeaponUI _weaponUI;
    [SerializeField] private ExperienceUI _experienceUI;
    [SerializeField] private StatsMultipliersUI _statsMultipliersUI;
    [SerializeField] private LevelProgressUI _levelProgressUI;

    private IHUDCompatible _iHUDCompatible;
    private LevelProgressManager _levelProgressManager;
    private bool _isInitialized;

    public void Init(IHUDCompatible iHUDCompatible, LevelProgressManager levelProgressManager)
    {
        _iHUDCompatible = iHUDCompatible;
        _levelProgressManager = levelProgressManager;
        _isInitialized = true;

        SubscribeEvents();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            SubscribeEvents();
        }
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _iHUDCompatible.onWeaponChanged += UpdateEquippedWeapon;
        _iHUDCompatible.onAmmoChanged += UpdateAmmo;
        _iHUDCompatible.onReloadProgressChanged += SetReloadBarValue;
        _iHUDCompatible.onLevelChanged += UpdatePlayerLevel;
        _iHUDCompatible.onExperienceProgressChanged += UpdateExperienceProgress;
        _iHUDCompatible.onStatsChanged += UpdateStats;

        _levelProgressManager.onKillProgressChnaged += UpdateLevelKillProgress;
        _levelProgressManager.onGameLevelChanged += SetGameLevel;
    }

    private void UnsubscribeEvents()
    {
        _iHUDCompatible.onWeaponChanged -= UpdateEquippedWeapon;
        _iHUDCompatible.onAmmoChanged -= UpdateAmmo;
        _iHUDCompatible.onLevelChanged -= UpdatePlayerLevel;
        _iHUDCompatible.onExperienceProgressChanged -= UpdateExperienceProgress;
        _iHUDCompatible.onStatsChanged -= UpdateStats;

        _levelProgressManager.onKillProgressChnaged -= UpdateLevelKillProgress;
        _levelProgressManager.onGameLevelChanged -= SetGameLevel;
    }

    private void UpdateEquippedWeapon(EWeaponType weapon)
    {
        for (int i = 0; i < _weaponUI.sprites.Length; i++)
        {
            var state = _weaponUI.sprites[i].name == weapon.ToString();
            _weaponUI.sprites[i].gameObject.SetActive(state);
        }
    }

    private void UpdateAmmo(int ammo)
    {
        _weaponUI.ammo.SetText(ammo.ToString());
    }

    private void UpdatePlayerLevel(int level)
    {
        _experienceUI.level.text = level.ToString();
    }

    private void UpdateExperienceProgress(float levelProgress)
    {
        _experienceUI.progress.text = ((int)(levelProgress * 100)).ToString() + " %";
        _experienceUI.progressBar.SetFill(levelProgress);
    }

    private void UpdateStats(SPlayerStatsMultipliers multipliers)
    {
        _statsMultipliersUI.damage.text = multipliers.damage.ToString("F1");
        _statsMultipliersUI.reload.text = multipliers.reload.ToString("F1");
        _statsMultipliersUI.ammo.text = multipliers.ammo.ToString("F1");
        _statsMultipliersUI.maxHealth.text = multipliers.maxHealth.ToString("F1");
        _statsMultipliersUI.healthRegen.text = multipliers.healthRegen.ToString("F1");
        _statsMultipliersUI.moveSpeed.text = multipliers.moveSpeed.ToString("F1");
    }

    private void SetReloadBarValue(float value)
    {
        _weaponUI.reloadBar.SetFill(value);
    }

    private void UpdateLevelKillProgress(int enemiesKilled, int levelGoal)
    {
        _levelProgressUI.levelKillProgress.text = $"KILLS {enemiesKilled}/{levelGoal}";
    }

    private void SetGameLevel(int level)
    {
        _levelProgressUI.gameLevel.text = $"LEVEL {level}";
    }

    [Serializable]
    private struct WeaponUI
    {
        public Image[] sprites;
        public TextMeshProUGUI ammo;
        public ReloadBar reloadBar;
    }

    [Serializable]
    private struct ExperienceUI
    {
        public TextMeshProUGUI level;
        public TextMeshProUGUI progress;
        public ExperienceBar progressBar;
    }

    [Serializable]
    private struct StatsMultipliersUI
    {
        public TextMeshProUGUI damage;
        public TextMeshProUGUI ammo;
        public TextMeshProUGUI reload;
        public TextMeshProUGUI maxHealth;
        public TextMeshProUGUI healthRegen;
        public TextMeshProUGUI moveSpeed;
    }
    
    [Serializable]
    private struct LevelProgressUI
    {
        public TextMeshProUGUI levelKillProgress;
        public TextMeshProUGUI gameLevel;
    }
}
