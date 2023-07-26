using System;
using UnityEngine;
using Structs;

public class EnemyHealthSystem : MonoBehaviour
{
    private float _baseHealth = 100;
    private float _baseHealthRegen = 10;

    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;

    private EnemyStatsSystem _statsSystem;
    private HealthBar _healthBar;

    public event Action onDamaged;
    public event Action onDie;
    
    public void Init(EnemyStatsSystem statsSystem)
    {
        _statsSystem = statsSystem;        
        CacheComponents();
        Subscribe();

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
        _healthBar = transform.Find("Canvas/HealthBar").GetComponent<HealthBar>();
    }

    public void ReduceHealth(float damage)
    {
        _health -= damage;
        _healthBar.SetFill(_health / _maxHealth);

        onDamaged.Invoke();
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
        _health = _maxHealth;
        _healthBar.SetFill(_health / _maxHealth);
        _regenPerSecond = _baseHealthRegen * stats.maxHealth;
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
