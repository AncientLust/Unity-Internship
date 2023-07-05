using System;
using UnityEngine;
using Structs;

public class EnemyStatsSystem : MonoBehaviour
{
    private EnemyExperienceSystem _experienceSystem; // Must be injected
    private SEnemyStatsMultipliers _multipliers;
    
    private struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    public event Action<SEnemyStatsMultipliers> onStatsChanged;

    private void Awake()
    {
        _experienceSystem = GetComponent<EnemyExperienceSystem>();
    }

    private void OnEnable()
    {
        _experienceSystem.OnLevelChanged += SetLevelStats;
    }

    private void OnDisable()
    {
        _experienceSystem.OnLevelChanged -= SetLevelStats;
    }

    private void SetLevelStats(int level)
    {
        level--; // Enemy level starts with 1, so when he reaches level 2, 1x growth will be given.

        _multipliers.damage = 1 + _levelUpGrowth.damage * level;
        _multipliers.maxHealth = 1 + _levelUpGrowth.maxHealth * level;
        _multipliers.healthRegen = 1 + _levelUpGrowth.healthRegen * level;
        _multipliers.moveSpeed = 1 + _levelUpGrowth.moveSpeed * level;
        onStatsChanged.Invoke(_multipliers);
    }
}
