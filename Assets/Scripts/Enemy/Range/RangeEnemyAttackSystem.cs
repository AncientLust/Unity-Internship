using UnityEngine;
using Structs;

public class RangeEnemyAttackSystem : MonoBehaviour
{
    protected float _baseDamage = 15;
    protected float _damage;
    protected EnemyStatsSystem _enemyStatsSystem;

    public void Init(EnemyStatsSystem enemyStatsSystem)
    {
        _enemyStatsSystem = enemyStatsSystem;
        _damage = _baseDamage;
        Subscribe();
    }

    protected void OnEnable()
    {
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void Subscribe()
    {
        if (_enemyStatsSystem != null) _enemyStatsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    protected void Unsubscribe() 
    {
        if (_enemyStatsSystem != null)  _enemyStatsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    protected void OnCollisionStay(Collision collision)
    {
        //var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isPlayer = collision.gameObject.GetComponent<PlayerFacade>();

        if (isPlayer != null)
        {
            LaunchRocket();
        }
    }

    protected void ApplyLevelUpMultipliers(SEnemyStatsMultipliers multipliers)
    {
        _damage = _baseDamage * multipliers.damage;
    }

    protected void LaunchRocket()
    {

    }
}
