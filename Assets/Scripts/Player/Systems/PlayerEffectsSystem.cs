using System.Collections;
using UnityEngine;

public class PlayerEffectsSystem : MonoBehaviour
{
    private ParticleSystem _bloodSplat;
    private ParticleSystem _levelUp;
    private ParticleSystem _bonusHealing;
    private ParticleSystem _bonusDamage;

    private PlayerHealthSystem _healthSystem;
    private PlayerExperienceSystem _experienceSystem;
    private BonusRegenerationSkill _bonusRegenerationSkill;
    private BonusDamageSkill _bonusDamageSkill;
    private GameSettings _gameSettings;

    public void Init(
        PlayerHealthSystem healthSystem, 
        PlayerExperienceSystem experienceSystem,
        BonusRegenerationSkill bonusRegenerationSkill,
        BonusDamageSkill bonusDamageSkill,
        GameSettings gameSettings)
    {
        _healthSystem = healthSystem;
        _experienceSystem = experienceSystem;
        _bonusRegenerationSkill = bonusRegenerationSkill;
        _bonusDamageSkill = bonusDamageSkill;
        _gameSettings = gameSettings;
        CacheComponents();
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void CacheComponents()
    {
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
        _levelUp = transform.Find("Effects/LevelUp").GetComponent<ParticleSystem>();
        _bonusHealing = transform.Find("Effects/BonusHealing").GetComponent<ParticleSystem>();
        _bonusDamage = transform.Find("Effects/BonusDamage").GetComponent<ParticleSystem>();
    }

    private void Subscribe()
    {
        _healthSystem.onDamaged += PlayBloodSplat;
        _experienceSystem.onLevelChanged += (level) => PlayerLevelUp(level);
        _bonusRegenerationSkill.onActivation += (duration, bonus) => StartCoroutine(PlayBonusHealing(duration));
        _bonusDamageSkill.onActivation += (duration, bonus) => StartCoroutine(PlayBonusDamage(duration));
    }

    private void Unsubscribe()
    {
        _healthSystem.onDamaged -= PlayBloodSplat;
        _experienceSystem.onLevelChanged -= (level) => PlayerLevelUp(level);
        _bonusRegenerationSkill.onActivation -= (duration, bonus) => StartCoroutine(PlayBonusHealing(duration));
        _bonusDamageSkill.onActivation -= (duration, bonus) => StartCoroutine(PlayBonusDamage(duration));
    }

    public void StopAllEffects()
    {
        _bloodSplat.Stop();
        _bloodSplat.Clear();
        _bonusHealing.Stop();
        _bonusHealing.Clear();
        _bonusDamage.Stop();
        _bonusDamage.Clear();
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

    private void PlayerLevelUp(int level)
    {
        if (level != 1)
        {
            _levelUp.Play();
        }
    }

    private IEnumerator PlayBonusHealing(float duration)
    {
        _bonusHealing.Play();
        yield return new WaitForSeconds(duration);
        _bonusHealing.Stop();
        _bonusHealing.Clear();
    }

    private IEnumerator PlayBonusDamage(float duration)
    {
        _bonusDamage.Play();
        yield return new WaitForSeconds(duration);
        _bonusDamage.Stop();
        _bonusDamage.Clear();
    }
}
