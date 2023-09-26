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
}
