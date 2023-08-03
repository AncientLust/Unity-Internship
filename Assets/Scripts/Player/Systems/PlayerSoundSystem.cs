using UnityEngine;
using Enums;

public class PlayerSoundSystem : MonoBehaviour
{
    private PlayerHealthSystem _healthSystem;
    private AudioPlayer _audioPlayer;
    private BonusDamageSkill _bonusDamageSkill;
    private BonusRegenerationSkill _bonusRegenerationSkill;
    private ThrowGrenadeSkill _throwGrenadeSkill;
    private PlayerWeaponSystem _weaponSystem;
    private bool _isInitialized = false;

    public void Init
    (
        AudioPlayer audioPlayer, 
        PlayerHealthSystem healthSystem,
        BonusDamageSkill bonusDamageSkill,
        BonusRegenerationSkill bonusRegenerationSkill,
        ThrowGrenadeSkill throwGrenadeSkill,
        PlayerWeaponSystem weaponSystem)
    {
        _audioPlayer = audioPlayer;
        _healthSystem = healthSystem;
        _bonusDamageSkill = bonusDamageSkill;
        _bonusRegenerationSkill = bonusRegenerationSkill;
        _throwGrenadeSkill = throwGrenadeSkill;
        _weaponSystem = weaponSystem;

        _isInitialized = true;
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

    private void Subscribe()
    {
        _healthSystem.onDie += () => _audioPlayer.PlaySound(ESound.PlayerDeath);
        _bonusDamageSkill.onActivation += (_, _) => _audioPlayer.PlaySound(ESound.RageEffect);
        _bonusRegenerationSkill.onActivation += (_, _) => _audioPlayer.PlaySound(ESound.HealthRegeneration);
        _throwGrenadeSkill.onActivation += () => _audioPlayer.PlaySound(ESound.GrenadeExplosion);
        _weaponSystem.onReload += () => _audioPlayer.PlaySound(ESound.Reload);
        _weaponSystem.onShoot += (shootSound) => _audioPlayer.PlaySound(shootSound);
    }

    private void Unsubscribe()
    {
        _healthSystem.onDie -= () => _audioPlayer.PlaySound(ESound.PlayerDeath);
        _bonusDamageSkill.onActivation -= (_, _) => _audioPlayer.PlaySound(ESound.RageEffect);
        _bonusRegenerationSkill.onActivation -= (_, _) => _audioPlayer.PlaySound(ESound.HealthRegeneration);
        _throwGrenadeSkill.onActivation -= () => _audioPlayer.PlaySound(ESound.GrenadeExplosion);
        _weaponSystem.onReload -= () => _audioPlayer.PlaySound(ESound.Reload);
        _weaponSystem.onShoot -= (shootSound) => _audioPlayer.PlaySound(shootSound);
    }
}
