using System.Collections;
using UnityEngine;

public class PlayerEffectsSystem : MonoBehaviour
{
    protected ParticleSystem _bloodSplat;
    protected ParticleSystem _levelUp;
    protected ParticleSystem _bonusHealing;
    protected ParticleSystem _bonusDamage;

    protected PlayerHealthSystem _healthSystem;
    protected PlayerExperienceSystem _experienceSystem;
    protected BonusRegenerationSkill _bonusRegenerationSkill;
    protected BonusDamageSkill _bonusDamageSkill;

    public void Init(
        PlayerHealthSystem healthSystem, 
        PlayerExperienceSystem experienceSystem,
        BonusRegenerationSkill bonusRegenerationSkill,
        BonusDamageSkill bonusDamageSkill)
    {
        _healthSystem = healthSystem;
        _experienceSystem = experienceSystem;
        _bonusRegenerationSkill = bonusRegenerationSkill;
        _bonusDamageSkill = bonusDamageSkill;
        CacheComponents();
        Subscribe();
    }

    protected void OnDisable()
    {
        Unsubscribe();
    }

    protected void CacheComponents()
    {
        _bloodSplat = transform.Find("Effects/BloodSplat").GetComponent<ParticleSystem>();
        _levelUp = transform.Find("Effects/LevelUp").GetComponent<ParticleSystem>();
        _bonusHealing = transform.Find("Effects/BonusHealing").GetComponent<ParticleSystem>();
        _bonusDamage = transform.Find("Effects/BonusDamage").GetComponent<ParticleSystem>();
    }

    protected void Subscribe()
    {
        _healthSystem.onDamaged += PlayBloodSplat;
        _experienceSystem.onLevelChanged += (level) => PlayerLevelUp(level);
        _bonusRegenerationSkill.onActivation += (duration, bonus) => StartCoroutine(PlayBonusHealing(duration));
        _bonusDamageSkill.onActivation += (duration, bonus) => StartCoroutine(PlayBonusDamage(duration));
    }

    protected void Unsubscribe()
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

    protected void PlayBloodSplat()
    {
        if (!_bloodSplat.isPlaying) // Ask SettingsSystem if enabled
        {
            _bloodSplat.Play();
        }
    }

    protected void PlayerLevelUp(int level)
    {
        if (level != 1) // Ask SettingsSystem if enabled
        {
            _levelUp.Play();
        }
    }

    protected IEnumerator PlayBonusHealing(float duration)
    {
        _bonusHealing.Play();
        yield return new WaitForSeconds(duration);
        _bonusHealing.Stop();
        _bonusHealing.Clear();
    }

    protected IEnumerator PlayBonusDamage(float duration)
    {
        _bonusDamage.Play();
        yield return new WaitForSeconds(duration);
        _bonusDamage.Stop();
        _bonusDamage.Clear();
    }
}
