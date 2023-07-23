using System;
using UnityEngine;
using Structs;

public class EnemyStatsSystem : MonoBehaviour
{
    protected EnemyExperienceSystem _experienceSystem;
    protected SEnemyStatsMultipliers _multipliers;
    
    protected struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    public event Action<SEnemyStatsMultipliers> onStatsChanged;

    public void Init(EnemyExperienceSystem levelsystem)
    {
        _experienceSystem = levelsystem;
        Subscribe();
    }

    protected void OnEnable()
    {
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubsribe();
    }

    protected void Subscribe()
    {
        if (_experienceSystem != null) _experienceSystem.OnLevelChanged += SetLevelStats;
    }

    protected void Unsubsribe()
    {
        if (_experienceSystem != null) _experienceSystem.OnLevelChanged -= SetLevelStats;
    }

    protected void SetLevelStats(int level)
    {
        level--; // Enemy level starts with 1, so when he reaches level 2, 1x growth will be given.

        _multipliers.damage = 1 + _levelUpGrowth.damage * level;
        _multipliers.maxHealth = 1 + _levelUpGrowth.maxHealth * level;
        _multipliers.healthRegen = 1 + _levelUpGrowth.healthRegen * level;
        _multipliers.moveSpeed = 1 + _levelUpGrowth.moveSpeed * level;
        onStatsChanged.Invoke(_multipliers);
    }
}
