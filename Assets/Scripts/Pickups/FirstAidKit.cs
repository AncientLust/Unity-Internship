using UnityEngine;
using Enums;

public class FirstAidKit : Pickup
{
    private float _healthHeal = 25;

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.gameObject.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        var iHealable = collider.gameObject.GetComponent<IHealable>();
        if (iHealable != null)
        {
            iHealable.Heal(_healthHeal);
            _iAudioPlayer.PlaySound(ESound.Pickup);
            _objectPool.Return(gameObject);
        }
    }
}
