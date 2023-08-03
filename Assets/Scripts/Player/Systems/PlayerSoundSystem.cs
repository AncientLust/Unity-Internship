using UnityEngine;
using Enums;

public class PlayerSoundSystem : MonoBehaviour
{
    private PlayerHealthSystem _healthSystem;
    private AudioPlayer _audioPlayer;

    public void Init(AudioPlayer audioPlayer, PlayerHealthSystem healthSystem)
    {
        _audioPlayer = audioPlayer;
        _healthSystem = healthSystem;
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
        if (_healthSystem != null) _healthSystem.onDie += () => _audioPlayer.PlaySound(ESound.PlayerDeath);
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= () => _audioPlayer.PlaySound(ESound.PlayerDeath);
    }
}
