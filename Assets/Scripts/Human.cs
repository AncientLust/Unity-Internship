using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected HealthBar _healthBar;
    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _healthRegen = 5f;
    [SerializeField] ParticleSystem _bloodSplat;

    public bool IsDead { get; protected set; }
    
    protected float _health;
    
    public void InitHealth()
    {
        _health = _maxHealth;
        _healthBar.SetHealth(1);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _healthBar.SetHealth(_health / _maxHealth);
        _bloodSplat.Play();
    }

    protected void Regenerate()
    {
        _health += _healthRegen * Time.deltaTime;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        _healthBar.SetHealth(_health / _maxHealth);
    }
}
