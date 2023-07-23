using UnityEngine;

public class EnemyEffectsSystem : MonoBehaviour
{
    protected ParticleSystem _bloodSplat;
    protected EnemyHealthSystem _healthSystem;

    protected bool _isInitialized = false;

    public void Init(EnemyHealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        CacheComponents();
        Subscribe();

        _isInitialized = true;
    }

    protected void OnEnable()
    {
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void CacheComponents()
    {
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
    }

    protected void Subscribe()
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

    protected void Unsubscribe()
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

    protected void PlayBloodSplat()
    {
        if (!_bloodSplat.isPlaying) // Ask SettingsSystem if enabled
        {
            _bloodSplat.Play();
        }
    }
}
