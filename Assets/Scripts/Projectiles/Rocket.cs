using UnityEngine;
using Enums;

public class Rocket : Projectile
{
    [SerializeField] Transform _explosionPoint;

    override protected void OnTriggerEnter(Collider collider)
    {
        var player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Exlode();
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

    private void Exlode()
    {
        _audioPlayer.PlaySound(ESound.Explosion);
        var explosion = _objectPool.Get(EResource.Explosion).GetComponent<Effect>();
        explosion.transform.position = _explosionPoint.position;
        explosion.Play();
    }
}
