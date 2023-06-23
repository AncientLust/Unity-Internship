using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodSplat;
    [SerializeField] protected HealthBar _healthBar;
    
    private StatsSystem _statsSystem;
    private IDamageable _damageable;

    public bool IsDead { get; set; }
    
    private void Awake()
    {
        CacheComponents();
        IsDead = false;
    }

    private void CacheComponents()
    {
        _statsSystem = GetComponent<StatsSystem>();
        _damageable = GetComponent<IDamageable>();
    }

    public void TakeDamage(float damage)
    {
        _statsSystem.CurrentHealth -= damage;
        _healthBar.SetFill(_statsSystem.CurrentHealth / _statsSystem.MaxHealth);
        PlayBloodEffect();
        CheckIfDied();
    }

    private void CheckIfDied()
    {
        if (_statsSystem.CurrentHealth <= 0)
        {
            IsDead = true;
            _damageable.Die();
        }
    }

    private void PlayBloodEffect()
    {
        //if (GameSettings.Instance.BloodEffect)
        //{
            _bloodSplat.Play();
        //}
    }

    public void Regenerate()
    {
        if (!IsDead)
        {
            _statsSystem.CurrentHealth += _statsSystem.HealthRegen * Time.deltaTime;
            _statsSystem.CurrentHealth = Mathf.Clamp(_statsSystem.CurrentHealth, 0, _statsSystem.MaxHealth);
            _healthBar.SetFill(_statsSystem.CurrentHealth / _statsSystem.MaxHealth);
            
            HideHealthIfHealthy();
        }
    }

    public void HideHealthIfHealthy()
    {
        _healthBar.gameObject.SetActive(_statsSystem.CurrentHealth != _statsSystem.MaxHealth);
    }
}
