using System;
using UnityEngine;
using Structs;
using System.Collections;

public class PlayerStatsSystem : MonoBehaviour, IMoveSpeedBoostable
{
    private BonusRegenerationSkill _bonusRegenerationSkill;
    private BonusDamageSkill _bonusDamageSkill;
    private PlayerExperienceSystem _experienceSystem;
    private SPlayerStatsMultipliers _multipliers;
    private int _level;
    private bool _isDamageAndReloadBonusActive = false;
    private bool _isRegenBonusActive = false;
    private bool _isMoveSpeedBonusActive = false;

    private struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float ammo = .05f;
        public const float reload = .95f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    public event Action<SPlayerStatsMultipliers> onStatsChanged;

    public void Init(
        PlayerExperienceSystem experienceSystem,
        BonusRegenerationSkill bonusRegenerationSkill,
        BonusDamageSkill doubleDamageSkill)
    {
        _experienceSystem = experienceSystem;
        _bonusRegenerationSkill = bonusRegenerationSkill;
        _bonusDamageSkill = doubleDamageSkill;

        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _experienceSystem.onLevelChanged += SetLevelStats;
        _bonusRegenerationSkill.onActivation += (duration, bonus) => StartCoroutine(ApplyRegenerationBonus(duration, bonus));
        _bonusDamageSkill.onActivation += (duration, bonus) => StartCoroutine(ApplyDamageAndReloadBonus(duration, bonus));
    }

    private void Unsubscribe()
    {
        _experienceSystem.onLevelChanged -= SetLevelStats;
        _bonusRegenerationSkill.onActivation -= (duration, bonus) => StartCoroutine(ApplyRegenerationBonus(duration, bonus));
        _bonusDamageSkill.onActivation -= (duration, bonus) => StartCoroutine(ApplyDamageAndReloadBonus(duration, bonus));
    }

    private void SetLevelStats(int level)
    {
        _level = level--; // Player level starts with 1, so when he reaches level 2, 1x growth will be given.

        _multipliers.ammo = GetLevelBasedAmmo();
        _multipliers.maxHealth = GetLevelBasedMaxHealth();
        _multipliers.damage = _isDamageAndReloadBonusActive ? _multipliers.damage : GetLevelBasedDamage();
        _multipliers.reload = _isDamageAndReloadBonusActive ? _multipliers.reload : GetLevelBasedReload();
        _multipliers.healthRegen = _isRegenBonusActive ? _multipliers.healthRegen : GetLevelBasedHealthRegen();
        _multipliers.moveSpeed = _isMoveSpeedBonusActive ? _multipliers.moveSpeed : GetLevelBasedMoveSpeed();
        
        onStatsChanged.Invoke(_multipliers);
    }

    public void BoostMoveSpeed(float duration, float bonus)
    {
        StartCoroutine(ApplyMoveSpeedBonus(duration, bonus));
    }

    private IEnumerator ApplyDamageAndReloadBonus(float duration, float bonus)
    {
        _isDamageAndReloadBonusActive = true;
        _multipliers.damage *= bonus;
        _multipliers.reload = 0;
        onStatsChanged.Invoke(_multipliers);
        yield return new WaitForSeconds(duration);
        _multipliers.damage = GetLevelBasedDamage();
        _multipliers.reload = GetLevelBasedReload();
        onStatsChanged.Invoke(_multipliers);
        _isDamageAndReloadBonusActive = false;
    }

    private IEnumerator ApplyRegenerationBonus(float duration, float bonus)
    {
        _isRegenBonusActive = true;
        _multipliers.healthRegen *= bonus;
        onStatsChanged.Invoke(_multipliers);
        yield return new WaitForSeconds(duration);
        _multipliers.healthRegen = GetLevelBasedHealthRegen();
        onStatsChanged.Invoke(_multipliers);
        _isRegenBonusActive = false;
    }

    public IEnumerator ApplyMoveSpeedBonus(float duration, float bonus)
    {
        _isMoveSpeedBonusActive = true;
        _multipliers.moveSpeed *= bonus;
        onStatsChanged.Invoke(_multipliers);
        yield return new WaitForSeconds(duration);
        _multipliers.moveSpeed = GetLevelBasedMoveSpeed();
        onStatsChanged.Invoke(_multipliers);
        _isMoveSpeedBonusActive = false;
    }

    private float GetLevelBasedDamage()
    {
        return 1 + _levelUpGrowth.damage * _level;
    }

    private float GetLevelBasedAmmo()
    {
        return 1 + _levelUpGrowth.ammo * _level;
    }

    private float GetLevelBasedReload()
    {
        return Mathf.Pow(_levelUpGrowth.reload, _level);
    }

    private float GetLevelBasedMaxHealth()
    {
        return 1 + _levelUpGrowth.maxHealth * _level;
    }

    private float GetLevelBasedHealthRegen()
    {
        return 1 + _levelUpGrowth.healthRegen * _level;
    }

    private float GetLevelBasedMoveSpeed()
    {
        return 1 + _levelUpGrowth.moveSpeed * _level;
    }
}
