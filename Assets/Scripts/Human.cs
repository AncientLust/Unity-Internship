using UnityEngine;

public class Human : MonoBehaviour
{
    [SerializeField] protected HealthBar _healthBar;
    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] protected float _moveSpeed = 5f;
    [SerializeField] protected float _healthRegen = 1f;
    public bool IsDead { get; private set; }
    
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

        if (_health <= 0)
        {
            IsDead = true;
            gameObject.SetActive(false);
        }
    }
}
