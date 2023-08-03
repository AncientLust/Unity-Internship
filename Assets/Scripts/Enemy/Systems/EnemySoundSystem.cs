using Enums;
using UnityEngine;

public class EnemySoundSystem : MonoBehaviour
{
    private EnemyHealthSystem _healthSystem;
    private AudioPlayer _audioPlayer;

    public void Init(AudioPlayer audioPlayer, EnemyHealthSystem healthSystem)
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
        if (_healthSystem != null) _healthSystem.onDie += () => _audioPlayer.PlaySound(ESound.EnemyDeath);
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= () => _audioPlayer.PlaySound(ESound.EnemyDeath);
    }
}
