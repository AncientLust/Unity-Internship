using System;
using UnityEngine;
using Structs;

public class EnemyStatsSystem : MonoBehaviour
{
    private EnemyExperienceSystem _experienceSystem;
    private SEnemyStatsMultipliers _multipliers;
    private bool _isInitialized;

    private struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float ammo = .05f;
        public const float reload = .95f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    public event Action<SEnemyStatsMultipliers> onStatsChanged;

    public void Init(EnemyExperienceSystem levelsystem)
    {
        _experienceSystem = levelsystem;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubsribe();
    }

    private void Subscribe()
    {
        _experienceSystem.OnLevelChanged += SetLevelStats;
    }

    private void Unsubsribe()
    {
        _experienceSystem.OnLevelChanged -= SetLevelStats;
    }

    private void SetLevelStats(int level)
    {
        level--; // Enemy level starts with 1, so when he reaches level 2, 1x growth will be given.

        _multipliers.damage = 1 + _levelUpGrowth.damage * level;
        _multipliers.ammo = 1 +_levelUpGrowth.ammo * level;
        _multipliers.reload = Mathf.Pow(_levelUpGrowth.reload, level);
        _multipliers.maxHealth = 1 + _levelUpGrowth.maxHealth * level;
        _multipliers.healthRegen = 1 + _levelUpGrowth.healthRegen * level;
        _multipliers.moveSpeed = 1 + _levelUpGrowth.moveSpeed * level;
        onStatsChanged.Invoke(_multipliers);
    }
}
