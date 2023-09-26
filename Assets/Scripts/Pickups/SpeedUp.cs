using Enums;
using UnityEngine;

public class SpeedUp : Pickup
{
    private float _duration = 5;
    private float _moveSpeedBonus = 1.5f;

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.gameObject.GetComponent<IPlayerFacade>();
        if (player == null)
        {
            return;
        }

        var speedBoostable = collider.gameObject.GetComponent<IMoveSpeedBoostable>();
        if (speedBoostable != null)
        {
            speedBoostable.BoostMoveSpeed(_duration, _moveSpeedBonus);
            _iAudioPlayer.PlaySound(ESound.Pickup);
            _objectPool.Return(gameObject);
        }
    }
}
