using UnityEngine;
using Structs;

public class MeleeEnemyAttackSystem : MonoBehaviour
{
    private float _baseDamage = 15;
    private float _damage;
    private EnemyStatsSystem _enemyStatsSystem;
    private bool _isInitialized;

    public void Init(EnemyStatsSystem enemyStatsSystem)
    {
        _enemyStatsSystem = enemyStatsSystem;
        _damage = _baseDamage;
        _isInitialized = true;
        
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _enemyStatsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void Unsubscribe() 
    {
        _enemyStatsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void OnCollisionStay(Collision collision)
    {
        var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isEnemy = collision.gameObject.GetComponent<EnemyFacade>();

        if (damagable != null && !isEnemy)
        {
            damagable.TakeDamage(_damage * Time.deltaTime);
        }
    }

    private void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _damage = _baseDamage * multipliers.damage;
    }
}
