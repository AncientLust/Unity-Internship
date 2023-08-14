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

    private void Exlode()
    {
        _iAudioPlayer.PlaySound(ESound.Explosion);
        var explosion = _objectPool.Get(EResource.Explosion).GetComponent<Effect>();
        explosion.transform.position = _explosionPoint.position;
        explosion.Play();
    }
}
