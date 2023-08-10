using UnityEngine;

public class Bullet : Projectile
{
    override protected void OnTriggerEnter(Collider collider)
    {
        var enemy = collider.gameObject.GetComponent<EnemyFacade>();
        if (enemy != null)
        {
            TryApplyPush(collider);
            TryApplyDamage(collider);
            PenetrationCheck();
        }
    }

    override protected void TryApplyDamage(Collider collider)
    {
        var damagable = collider.gameObject.GetComponent<IDamageable>();
        if (damagable != null)
        {
            damagable.TakeDamage(_damage);
        }
    }

    override protected void TryApplyPush(Collider collider)
    {
        var pushiable = collider.gameObject.GetComponent<IPushiable>();
        if (pushiable != null)
        {
            pushiable.Push(transform.forward * _pushPower);
        }
    }

    override protected void PenetrationCheck()
    {
        if (!IsPenetratiable)
        {
            _objectPool.Return(gameObject);
        }
    }
}
