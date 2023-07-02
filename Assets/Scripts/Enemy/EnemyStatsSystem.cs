using System;
using UnityEngine;

public class EnemyStatsSystem : MonoBehaviour
{
    private EnemyExperienceSystem _experienceSystem; // Must be injected
    private EnemyStatsMultipliers _multipliers;
    
    private struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    public event Action<EnemyStatsMultipliers> onStatsChanged;

    private void SetLevelStats(int level)
    {
        _multipliers.damage = 1 + _levelUpGrowth.damage * level;
        _multipliers.maxHealth = 1 + _levelUpGrowth.maxHealth * level;
        _multipliers.healthRegen = 1 + _levelUpGrowth.healthRegen * level;
        _multipliers.moveSpeed = 1 + _levelUpGrowth.moveSpeed * level;
        onStatsChanged.Invoke(_multipliers);
    }

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
}
