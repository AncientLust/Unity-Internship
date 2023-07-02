using UnityEngine;

public class EnemyAttackSystem : MonoBehaviour
{
    private float _baseDamage = 10;
    private float _damage;

    private EnemyStatsSystem _enemyStatsSystem; // Must be injected

    private void Awake()
    {
        _enemyStatsSystem = GetComponent<EnemyStatsSystem>();
    }

    private void Start()
    {
        _damage = _baseDamage;
    }

    private void OnEnable()
    {
        _enemyStatsSystem.onStatsChanged += ApplyLevelUpMultipliers;
    }

    private void OnDisable()
    {
        _enemyStatsSystem.onStatsChanged -= ApplyLevelUpMultipliers;
    }

    private void OnCollisionStay(Collision collision)
    {
        var damagable = collision.gameObject.GetComponent<IDamageable>();
        var isAnotherEnemy = collision.gameObject.GetComponent<EnemyFacade>();

        if (damagable != null && !isAnotherEnemy)
        {
            damagable.TakeDamage(_damage * Time.deltaTime);
        }
    }

    private void ApplyLevelUpMultipliers(EnemyStatsMultipliers multipliers)
    {
        _damage = _baseDamage * multipliers.damage;
    }
}
