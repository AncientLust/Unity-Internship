using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem _bloodSplat;
    [SerializeField] protected HealthBar _healthBar;
    
    protected float _maxHealth = 100;
    protected float _healthRegen = 5f;

    public bool IsDead { get; protected set; }
    
    protected float _health;
    
    public void InitHealth()
    {
        _health = _maxHealth;
        _healthBar.SetHealthFill(1);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _healthBar.SetHealthFill(_health / _maxHealth);
        _bloodSplat.Play();
    }

    protected void Regenerate()
    {
        _health += _healthRegen * Time.deltaTime;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthBar.SetHealthFill(_health / _maxHealth);
    }
}
