using UnityEngine;

public class EnemyEffectsSystem : MonoBehaviour
{
    private ParticleSystem _bloodSplat;
    private EnemyHealthSystem _healthSystem;
    private GameSettings _gameSettings;
    private bool _isInitialized = false;

    public void Init(EnemyHealthSystem healthSystem, GameSettings gameSettings)
    {
        _healthSystem = healthSystem;
        _gameSettings = gameSettings;
        _isInitialized = true;

        CacheComponents();
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void CacheComponents()
    {
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
    }

    private void Subscribe()
    {
        _healthSystem.onDamaged += PlayBloodSplat;
    }

    private void Unsubscribe()
    {
        _healthSystem.onDamaged -= PlayBloodSplat;
    }

    public void StopAllEffects()
    {
        _bloodSplat.Stop();
        _bloodSplat.Clear();
    }

    private void PlayBloodSplat()
    {
        if (_gameSettings.IsBloodEffectEnabled)
        {
            if (!_bloodSplat.isPlaying)
            {
                _bloodSplat.Play();
            }
        }
    }
}
