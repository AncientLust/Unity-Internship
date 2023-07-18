using System;
using UnityEngine;
using Structs;

public class PlayerHealthSystem : MonoBehaviour
{
    private float _baseHealth = 100;
    private float _baseHealthRegen = 5;
    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;

    private PlayerExperienceSystem _experienceSystem;
    private PlayerStatsSystem _statsSystem;
    private ParticleSystem _bloodSplat;
    private HealthBar _healthBar;

    public event Action onDie;

    public float Health { get { return _health; } set { _health = value; } }

    public void Init(PlayerStatsSystem statsSystem, PlayerExperienceSystem experienceSystem)
    {
        _statsSystem = statsSystem;
        _experienceSystem = experienceSystem;
        Subscribe();
    }

    private void Awake()
    {
        CacheComponents();
        _isDead = false;
    }

    private void Start()
    {
        _maxHealth = _baseHealth;
        _health = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
    }

    private void Update()
    {
        Regenerate();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _statsSystem.onStatsChanged += ApplyStatsMultipliers;
        _experienceSystem.onLevelChanged += (_) => RestoreHealth();
    
    }

    private void Unsubscribe()
    {
        _statsSystem.onStatsChanged -= ApplyStatsMultipliers;
        _experienceSystem.onLevelChanged -= (_) => RestoreHealth();
    }

    private void CacheComponents()
    {
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
        _healthBar = transform.Find("Canvas/HealthBar").GetComponent<HealthBar>();
    }

    public void ReduceHealth(float damage)
    {
        _health -= damage;
        _healthBar.SetFill(_health / _maxHealth);

        PlayBloodEffect();
        CheckIfDied();
    }

    private void CheckIfDied()
    {
        if (_health <= 0)
        {
            _isDead = true;
            onDie.Invoke();
        }
    }

    private void PlayBloodEffect()
    {
        if (!_bloodSplat.isPlaying) // Ask SettingsSystem if enabled
        {
            //if (GameSettings.Instance.BloodEffect)
            //{
            _bloodSplat.Play();
            //}
        }
    }

    public void Regenerate()
    {
        if (!_isDead)
        {
            _health += _regenPerSecond * Time.deltaTime;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _healthBar.SetFill(_health / _maxHealth);

            HideHealthIfHealthy();
        }
    }

    public void HideHealthIfHealthy()
    {
        _healthBar.gameObject.SetActive(_health != _maxHealth);
    }

    private void ApplyStatsMultipliers(SPlayerStatsMultipliers stats)
    {
        _maxHealth = _baseHealth * stats.maxHealth;
        _regenPerSecond = _baseHealthRegen * stats.healthRegen;
    }

    private void RestoreHealth()
    {
        _health = _maxHealth;
        _healthBar.SetFill(1);
    }

    public void ResetHealth()
    {
        _isDead = false;
        _health = _baseHealth;
        _maxHealth = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }
}
