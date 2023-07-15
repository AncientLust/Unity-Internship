using System;
using UnityEngine;
using Structs;

public class EnemyHealthSystem : MonoBehaviour, IDamageable, IDisposable, IHealable
{
    private float _baseHealth = 100;
    private float _baseHealthRegen = 10;

    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;

    private EnemyStatsSystem _statsSystem;
    private ParticleSystem _bloodSplat;
    private HealthBar _healthBar;

    public event Action<GameObject> OnDispose;

    public void Init(EnemyStatsSystem statsSystem)
    {
        _statsSystem = statsSystem;
        Subscribe();
        CacheComponents(); // Must be refactored via ParticleSystemClass
    }

    private void Start()
    {
        _isDead = false;
        _health = _baseHealth;
        _maxHealth = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void Unsubscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void Update()
    {
        Regenerate();
    }

    private void CacheComponents()
    {
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
            OnDispose.Invoke(gameObject);
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

    private void ApplyLevelUpMultipliers(SEnemyStatsMultipliers stats)
    {
        _maxHealth = _baseHealth * stats.maxHealth;
        _regenPerSecond = _baseHealthRegen * stats.maxHealth;
    }

    public void RestoreHealth()
    {
        _isDead = false;
        _health = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }
}
