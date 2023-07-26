using UnityEngine;

public class EnemyEffectsSystem : MonoBehaviour
{
    private ParticleSystem _bloodSplat;
    private EnemyHealthSystem _healthSystem;

    private bool _isInitialized = false;

    public void Init(EnemyHealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        CacheComponents();
        Subscribe();

        _isInitialized = true;
    }

    private void OnEnable()
    {
        Subscribe();
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
        if (_healthSystem != null)
        {
            _healthSystem.onDamaged += PlayBloodSplat;
        }

        if (_isInitialized && _healthSystem == null)
        {
            Debug.LogError("Empty");
        }
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null)
        {
            _healthSystem.onDamaged -= PlayBloodSplat;
        }
    }

    public void StopAllEffects()
    {
        _bloodSplat.Stop();
        _bloodSplat.Clear();
    }

    private void PlayBloodSplat()
    {
        if (!_bloodSplat.isPlaying) // Ask SettingsSystem if enabled
        {
            _bloodSplat.Play();
        }
    }
}
