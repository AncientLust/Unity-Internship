using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private ParticleSystem _bloodSplat;
    [SerializeField] protected HealthBar _healthBar;

    [SerializeField] private float _baseHealth;
    [SerializeField] private float _baseHealthRegen;

    private StatsSystem _statsSystem;
    private IDamageable _damageable;

    private float _maxHealth;
    private float _health;
    private float _regenPerSecond;
    private bool _isDead;
    
    private void Awake()
    {
        CacheComponents();
        _isDead = false;
    }

    private void Start()
    {
        _maxHealth = _baseHealth;
        _health = _maxHealth;
        _regenPerSecond = _baseHealthRegen;
    }

    private void OnEnable()
    {
        _statsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void Update()
    {
        Regenerate();
    }

    private void OnDisable()
    {
        _statsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void CacheComponents()
    {
        _statsSystem = GetComponent<StatsSystem>();
        _damageable = GetComponent<IDamageable>();
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
            Die();
        }
    }

    private void Die()
    {
        // This definitely must be refactored
        if (gameObject.CompareTag(Tags.Player.ToString()))
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            ObjectPool.Instance.Return(gameObject);
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

    private void ApplyLevelUpMultipliers(StatsMultipliers stats)
    {
        _maxHealth = _baseHealth * stats.maxHealth;
        _regenPerSecond = _baseHealthRegen * stats.maxHealth;
    }

    public void ResetHealth()
    {
        _isDead = false;
        _health = _baseHealth;
        _regenPerSecond = _baseHealthRegen;
        _healthBar.SetFill(1);
    }
}
