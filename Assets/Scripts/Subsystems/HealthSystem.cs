using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodSplat;
    [SerializeField] protected HealthBar _healthBar;
    
    private StatsSystem _statsSystem;

    public bool IsDead { get; protected set; }
    
    private void Start()
    {
        CacheComponents();
    }

    private void CacheComponents()
    {
        _statsSystem = gameObject.GetComponent<StatsSystem>();
    }

    public bool TakeDamageTrueIfFatal(float damage)
    {
        if (_statsSystem.CurrentHealth - damage > 0)
        {
            _statsSystem.CurrentHealth -= damage;
            _healthBar.SetFill(_statsSystem.CurrentHealth / _statsSystem.MaxHealth);
            PlayBloodEffect();
            return false;
        }
        else
        {
            IsDead = true;
            gameObject.SetActive(false);
            return true;
        }
    }

    private void PlayBloodEffect()
    {
        if (GameSettings.Instance.BloodEffect)
        {
            _bloodSplat.Play();
        }
    }

    public void Regenerate()
    {
        _statsSystem.CurrentHealth += _statsSystem.HealthRegen * Time.deltaTime;
        _statsSystem.CurrentHealth = Mathf.Clamp(_statsSystem.CurrentHealth, 0, _statsSystem.MaxHealth);
        _healthBar.SetFill(_statsSystem.CurrentHealth / _statsSystem.MaxHealth);
    }

    public void HideHealthIfHealthy()
    {
        _healthBar.gameObject.SetActive(_statsSystem.CurrentHealth != _statsSystem.MaxHealth);
    }
}
