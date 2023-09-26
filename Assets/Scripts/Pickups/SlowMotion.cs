using Enums;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class Slowmotion : Pickup
{
    private float _slowMotionScale = 0.5f;
    private float _durationInSeconds = 5;

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.gameObject.GetComponent<Player>();
        if (player != null)
        {
            SlowMotion().Forget();
            _iAudioPlayer.PlaySound(ESound.Pickup);
            _objectPool.Return(gameObject);
        }
    }

    private async UniTask SlowMotion()
    {
        Time.timeScale = _slowMotionScale;
        await UniTask.Delay(TimeSpan.FromSeconds(_durationInSeconds * _slowMotionScale));
        Time.timeScale = 1f;
    }
}
