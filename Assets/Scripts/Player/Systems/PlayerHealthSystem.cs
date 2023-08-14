using System;
using UnityEngine;
using Structs;
using System.Collections;
using Enums;

public class PlayerHealthSystem : MonoBehaviour
{
    private float _baseHealth = 100;
    private float _baseHealthRegen = 5;
    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;
    private WaitForSeconds _deathDelay = new WaitForSeconds(1.5f);

    private PlayerExperienceSystem _experienceSystem;
    private PlayerStatsSystem _statsSystem;
    private HealthBar _healthBar;
    private IAudioPlayer _iAudioPlayer;

    public event Action onDie;
    public event Action onDamaged;
    public event Action onDied;

    public float Health { get { return _health; } set { _health = value; } }

    public void Init(PlayerStatsSystem statsSystem, PlayerExperienceSystem experienceSystem, IAudioPlayer iAudioPlayer)
    {
        _statsSystem = statsSystem;
        _experienceSystem = experienceSystem;
        _iAudioPlayer = iAudioPlayer;
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
        _healthBar = transform.Find("Canvas/HealthBar").GetComponent<HealthBar>();
    }

    public void ReduceHealth(float damage)
    {
        if (!_isDead)
        {
            _health -= damage;
            _healthBar.SetFill(_health / _maxHealth);
            onDamaged.Invoke();
            CheckIfDied();
        }
    }

    public void AddHealth(float health)
    {
        if (!_isDead)
        {
            _health += health;
            _healthBar.SetFill(_health / _maxHealth);
        }
    }

    private void CheckIfDied()
    {
        if (_health <= 0)
        {
            _isDead = true;
            _iAudioPlayer.PlaySound(ESound.PlayerDeath);
            onDie.Invoke();
            StartCoroutine(DelayedDeath());
        }
    }

    public void Regenerate()
    {
        if (!_isDead)
        {
            _health += _regenPerSecond * Time.deltaTime;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            _healthBar.SetFill(_health / _maxHealth);
        }
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

    public IEnumerator DelayedDeath()
    {
        yield return _deathDelay;
        onDied.Invoke();
    }
}
