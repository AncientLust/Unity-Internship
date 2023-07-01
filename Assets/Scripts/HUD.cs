using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private WeaponUI _weaponUI;
    [SerializeField] private ExperienceUI _experienceUI;
    [SerializeField] private StatsMultipliersUI _statsMultipliersUI;

    private Player _player;
    private PlayerExperienceSystem _playerExperienceSystem;
    private StatsSystem _statsSystem;
    private WeaponSystem _weaponSystem;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(); // Must be injected via constructor
        _playerExperienceSystem = _player.GetComponent<PlayerExperienceSystem>();
        _statsSystem = _player.GetComponent<StatsSystem>();
        _weaponSystem = _player.GetComponent<WeaponSystem>();
    }

    private void OnEnable()
    {
        _weaponSystem.onWeaponChanged += SetWeaponInfo;
        _weaponSystem.onAmmoChanged += SetAmmoInfo;
        _playerExperienceSystem.OnLevelChanged += UpdateLevelInfo;
        _playerExperienceSystem.OnExperienceChanged += UpdateExperienceProgress;
        _statsSystem.onStatsChanged += UpdateStats;
    }

    private void OnDisable()
    {
        _weaponSystem.onWeaponChanged -= SetWeaponInfo;
        _weaponSystem.onAmmoChanged -= SetAmmoInfo;
        _playerExperienceSystem.OnLevelChanged -= UpdateLevelInfo;
        _playerExperienceSystem.OnExperienceChanged -= UpdateExperienceProgress;
        _statsSystem.onStatsChanged -= UpdateStats;
    }

    private void SetWeaponInfo(string weapon)
    {
        for (int i = 0; i < _weaponUI.sprites.Length; i++)
        {
            var state = _weaponUI.sprites[i].name == weapon;
            _weaponUI.sprites[i].gameObject.SetActive(state);
        }
    }

    private void SetAmmoInfo(int ammo)
    {
        _weaponUI.ammo.SetText($"999 / {ammo}");
    }

    private void UpdateLevelInfo(int level)
    {
        _experienceUI.level.text = level.ToString();
    }

    private void UpdateExperienceProgress(float levelPercent)
    {
        _experienceUI.progress.text = ((int)(levelPercent * 100)).ToString() + " %";
        _experienceUI.progressBar.SetFill(levelPercent);
    }

    private void UpdateStats(StatsMultipliers multipliers)
    {
        _statsMultipliersUI.damage.text = multipliers.damage.ToString("F1");
        _statsMultipliersUI.reload.text = multipliers.reload.ToString("F1");
        _statsMultipliersUI.ammo.text = multipliers.ammo.ToString("F1");
        _statsMultipliersUI.maxHealth.text = multipliers.maxHealth.ToString("F1");
        _statsMultipliersUI.healthRegen.text = multipliers.healthRegen.ToString("F1");
        _statsMultipliersUI.moveSpeed.text = multipliers.moveSpeed.ToString("F1");
    }

    [System.Serializable]
    private struct WeaponUI
    {
        public Image[] sprites;
        public TextMeshProUGUI ammo;
    }

    [System.Serializable]
    private struct ExperienceUI
    {
        public TextMeshProUGUI level;
        public TextMeshProUGUI progress;
        public ExperienceBar progressBar;
    }

    [System.Serializable]
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
