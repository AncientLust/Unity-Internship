using TMPro;
using UnityEngine;

public class StatsSystem : MonoBehaviour
{
    [SerializeField] private float _initialHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _initialHealthRegen;
    [SerializeField] private float _initialMoveSpeed;

    [SerializeField] private TextMeshProUGUI _powerMultiplier;
    [SerializeField] private TextMeshProUGUI _ammoMultiplier;
    [SerializeField] private TextMeshProUGUI _reloadMultiplier;

    private float _powerLevelIncrement = 0.05f;
    private float _ammoLevelIncrement = 0.05f;
    private float _reloadLevelIncrement = 0.95f;
    private float _speedLevelIncrement = 0.01f;
    private float _healthLevelIncrement = 0.05f;

    private string _player = "Player";

    public float PowerMultiplier { get; private set; } = 1;
    public float AmmoMultiplier { get; private set; } = 1;
    public float ReloadMultiplier { get; private set; } = 1;
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; set; }
    public float HealthRegen { get; private set; } 
    public float MoveSpeed { get; private set; }
    public int Level { get; private set; }

    private void Start()
    {
        InitStats();
    }

    private void InitStats()
    {
        Level = 1;
        MaxHealth = _initialHealth;
        CurrentHealth = _initialHealth;
        HealthRegen = _initialHealthRegen;
        MoveSpeed = _initialMoveSpeed;
    }

    public void SetLevelStats(int level)
    {
        PowerMultiplier = 1 + _powerLevelIncrement * level;
        AmmoMultiplier = 1 + _ammoLevelIncrement * level;
        ReloadMultiplier = Mathf.Pow(_reloadLevelIncrement, level);

        MaxHealth = _initialHealth * (1 + _speedLevelIncrement * level);
        HealthRegen = _initialHealthRegen * (1 + _speedLevelIncrement * level);
        MoveSpeed = _initialMoveSpeed * (1 + _healthLevelIncrement * level);

        CurrentHealth = MaxHealth;

        UpdatePlayerUIStats();
    }

    public void UpdatePlayerUIStats()
    {
        if (gameObject.name == _player)
        {
            _powerMultiplier.text = PowerMultiplier.ToString("F1");
            _ammoMultiplier.text = AmmoMultiplier.ToString("F1");
            _reloadMultiplier.text = ReloadMultiplier.ToString("F1");
        }
    }
}
