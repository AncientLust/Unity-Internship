using System;
using UnityEngine;
using Structs;
using System.Collections;

public class PlayerStatsSystem : MonoBehaviour
{
    private BonusRegenerationSkill _bonusRegenerationSkill;
    private BonusDamageSkill _bonusDamageSkill;
    private PlayerExperienceSystem _experienceSystem;
    private SPlayerStatsMultipliers _multipliers;
    private int _level;
    private bool _isBonusDamageActive = false;
    private bool _isBonusRegenActive = false;

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
        _bonusDamageSkill.onActivation += (duration, bonus) => StartCoroutine(ApplyDamageBonus(duration, bonus));

    }

    private void Unsubscribe()
    {
        _experienceSystem.onLevelChanged -= SetLevelStats;
        _bonusRegenerationSkill.onActivation -= (duration, bonus) => StartCoroutine(ApplyRegenerationBonus(duration, bonus));
        _bonusDamageSkill.onActivation -= (duration, bonus) => StartCoroutine(ApplyDamageBonus(duration, bonus));
    }

    private void SetLevelStats(int level)
    {
        _level = level--; // Player level starts with 1, so when he reaches level 2, 1x growth will be given.

        if (!_isBonusRegenActive && !_isBonusRegenActive)
        {
            _multipliers.damage = GetLevelBasedDamage();
            _multipliers.ammo = GetLevelBasedAmmo();
            _multipliers.reload = GetLevelBasedReload();
            _multipliers.maxHealth = GetLevelBasedMaxHealth();
            _multipliers.healthRegen = GetLevelBasedHealthRegen();
            _multipliers.moveSpeed = GetLevelBasedMoveSpeed();
            onStatsChanged.Invoke(_multipliers);
        }
    }

    private IEnumerator ApplyDamageBonus(float duration, float bonus)
    {
        _isBonusDamageActive = true;
        _multipliers.damage *= bonus;
        onStatsChanged.Invoke(_multipliers);
        yield return new WaitForSeconds(duration);
        _multipliers.damage = GetLevelBasedDamage();
        onStatsChanged.Invoke(_multipliers);
        _isBonusDamageActive = false;

        if (!_isBonusRegenActive)
        {
            SetLevelStats(_level);
        }
    }

    private IEnumerator ApplyRegenerationBonus(float duration, float bonus)
    {
        _isBonusRegenActive = true;
        _multipliers.healthRegen *= bonus;
        onStatsChanged.Invoke(_multipliers);
        yield return new WaitForSeconds(duration);
        _multipliers.healthRegen = GetLevelBasedHealthRegen();
        onStatsChanged.Invoke(_multipliers);
        _isBonusRegenActive = false;

        if (!_isBonusDamageActive)
        {
            SetLevelStats(_level);
        }
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
