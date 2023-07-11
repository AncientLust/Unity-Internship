using System;
using UnityEngine;
using Structs;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    private float _baseHealth = 100;
    private float _baseHealthRegen = 5;
    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;

    private PlayerStatsSystem _statsSystem; // Must be injected
    private ParticleSystem _bloodSplat;
    private HealthBar _healthBar;

    public event Action OnDie;

    public void Init(PlayerStatsSystem statsSystem)
    {
        _statsSystem = statsSystem;
        SubscribeEvents();
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
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void UnsubscribeEvents()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void CacheComponents()
    {
        //_statsSystem = GetComponent<PlayerStatsSystem>();
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
        _healthBar = transform.Find("Canvas/HealthBar").GetComponent<HealthBar>();
    }

    public void TakeDamage(float damage)
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
            OnDie.Invoke();
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

    private void ApplyLevelUpMultipliers(SPlayerStatsMultipliers stats)
    {
        _health = _maxHealth = _baseHealth * stats.maxHealth;
        _regenPerSecond = _baseHealthRegen * stats.maxHealth;
        RestoreHealth();
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
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }
}