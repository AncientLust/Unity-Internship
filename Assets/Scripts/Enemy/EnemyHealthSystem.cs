using System;
using UnityEngine;
using Structs;

public class EnemyHealthSystem : MonoBehaviour
{
    protected float _baseHealth = 100;
    protected float _baseHealthRegen = 10;

    protected float _maxHealth;
    protected float _health;
    protected float _regenPerSecond;
    protected bool _isDead;

    protected EnemyStatsSystem _statsSystem;
    protected HealthBar _healthBar;

    public event Action OnDie;
    public event Action onDamaged;
    
    public void Init(EnemyStatsSystem statsSystem)
    {
        _statsSystem = statsSystem;
        Subscribe();
        CacheComponents();
    }

    protected void Start()
    {
        _isDead = false;
        _health = _baseHealth;
        _maxHealth = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }

    protected void OnEnable()
    {
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void Subscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    protected void Unsubscribe()
    {
        if (_statsSystem != null) _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    protected void Update()
    {
        Regenerate();
    }

    protected void CacheComponents()
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

    protected void CheckIfDied()
    {
        if (_health <= 0)
        {
            _isDead = true;
            OnDie.Invoke();
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

    protected void ApplyLevelUpMultipliers(SEnemyStatsMultipliers stats)
    {
        _maxHealth = _baseHealth * stats.maxHealth;
        _regenPerSecond = _baseHealthRegen * stats.maxHealth;
    }

    public void RestoreHealth()
    {
        _isDead = false;
        _health = _baseHealth;
        _maxHealth = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }
}
