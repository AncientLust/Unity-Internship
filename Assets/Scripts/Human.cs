using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected HealthBar _healthBar;
    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] protected float _moveSpeed = 5f;
    public bool IsDead { get; private set; }
    
    protected float _minHealth = 0;
    protected float _health;
    
    public void InitHealth()
    {
        _health = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _healthBar.SetHealth(_health);

        if (_health <= _minHealth)
        {
            IsDead = true;
            gameObject.SetActive(false);
        }
    }
}
