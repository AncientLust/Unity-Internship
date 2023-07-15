using UnityEngine;
using Structs;

public class EnemyAttackSystem : MonoBehaviour
{
    private float _baseDamage = 15;
    private float _damage;
    private EnemyStatsSystem _enemyStatsSystem;

    public void Init(EnemyStatsSystem enemyStatsSystem)
    {
        _enemyStatsSystem = enemyStatsSystem;
        _damage = _baseDamage;
        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_enemyStatsSystem != null) _enemyStatsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void Unsubscribe() 
    {
        if (_enemyStatsSystem != null)  _enemyStatsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void OnCollisionStay(Collision collision)
    {
        var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isEnemy = collision.gameObject.GetComponent<Enemy>();

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
