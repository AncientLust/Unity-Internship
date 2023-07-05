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

    private PlayerFacade _playerFacade;

    public void Init(PlayerFacade playerFacade)
    {
        _playerFacade = playerFacade;
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _playerFacade.onWeaponChanged += UpdateEquippedWeapon;
        _playerFacade.onAmmoChanged += UpdateAmmo;
        _playerFacade.onLevelChanged += UpdatePlayerLevel;
        _playerFacade.onExperienceProgressChanged += UpdateExperienceProgress;
        _playerFacade.onStatsChanged += UpdateStats;
    }

    private void UnsubscribeEvents()
    {
        _playerFacade.onWeaponChanged -= UpdateEquippedWeapon;
        _playerFacade.onAmmoChanged -= UpdateAmmo;
        _playerFacade.onLevelChanged -= UpdatePlayerLevel;
        _playerFacade.onExperienceProgressChanged -= UpdateExperienceProgress;
        _playerFacade.onStatsChanged -= UpdateStats;
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
        _weaponUI.ammo.SetText($"999 / {ammo}");
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

    [Serializable]
    private struct WeaponUI
    {
        public Image[] sprites;
        public TextMeshProUGUI ammo;
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
}
