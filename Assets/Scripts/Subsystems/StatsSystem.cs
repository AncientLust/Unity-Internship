using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private float _initialMoveSpeed;
    //[SerializeField] private float _currentHealth;

    //[SerializeField] private TextMeshProUGUI _damageMultiplier;
    //[SerializeField] private TextMeshProUGUI _ammoMultiplier;
    //[SerializeField] private TextMeshProUGUI _reloadMultiplier;

    private struct _levelUpGrowth
    {
        public const float damage = .05f;
        public const float ammo = .05f;
        public const float reload = .95f;
        public const float maxHealth = .05f;
        public const float healthRegen = .05f;
        public const float moveSpeed = .01f;
    }

    private StatsMultipliers _multipliers;

    //public float DamageMultiplier { get; private set; } = 1; // Event to weapon system
    //public float AmmoMultiplier { get; private set; } = 1; // Event to weapon system
    //public float ReloadMultiplier { get; private set; } = 1; // Event to weapon system
    //public float MaxHealth { get; private set; } // Could be put out of stats system?
    //public float CurrentHealth { get; set; } // Could be put out of stats system?
    //public float HealthRegen { get; private set; } // Could be put out of stats system?
    public float MoveSpeed { get; private set; } // Event to move system

    public delegate void OnStatsChangedHandler(StatsMultipliers statsMultipliers);
    public event OnStatsChangedHandler onStatsChanged;  

    private void Start()
    {
        InitStats();
    }

    private void InitStats()
    {
        MoveSpeed = _initialMoveSpeed;
    }

    public void SetLevelStats(int level)
    {
        UpdateStatsMultipliers(level);
        UpdatePublicStats();
    }

    private void UpdateStatsMultipliers(int level)
    {
        _multipliers.damage = 1 + _levelUpGrowth.damage * level;
        _multipliers.ammo = 1 + _levelUpGrowth.ammo * level;
        _multipliers.reload = Mathf.Pow(_levelUpGrowth.reload, level);
        _multipliers.maxHealth = 1 + _levelUpGrowth.maxHealth * level;
        _multipliers.healthRegen = 1 + _levelUpGrowth.healthRegen * level;
        _multipliers.moveSpeed = 1 + _levelUpGrowth.moveSpeed * level;
    }

    private void UpdatePublicStats()
    {
        //CurrentHealth = _initialHealth * _multipliers.maxHealth;
        //MaxHealth = _initialHealth * _multipliers.maxHealth;
        //HealthRegen = _initialHealthRegen * _multipliers.healthRegen;
        MoveSpeed = _initialMoveSpeed * _multipliers.moveSpeed;

        RaiseStatsChangeEvent();
    }

    public void RaiseStatsChangeEvent()
    {
        if (gameObject.CompareTag(Tags.Player.ToString()))
        {
            onStatsChanged.Invoke(_multipliers);
        }
    }
}
