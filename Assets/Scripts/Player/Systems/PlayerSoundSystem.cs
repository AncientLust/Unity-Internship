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
        if (_bonusDamageSkill != null) _bonusDamageSkill.onActivation += (_, _) => _audioPlayer.PlaySound(ESound.RageEffect);
        if (_bonusRegenerationSkill != null) _bonusRegenerationSkill.onActivation += (_, _) => _audioPlayer.PlaySound(ESound.HealthRegeneration);
        if (_throwGrenadeSkill != null) _throwGrenadeSkill.onActivation += () => _audioPlayer.PlaySound(ESound.GrenadeThrow);
        if (_weaponSystem != null) _weaponSystem.onReload += () => _audioPlayer.PlaySound(ESound.Reload);
        if (_weaponSystem != null) _weaponSystem.onShoot += (shootSound) => _audioPlayer.PlaySound(shootSound);
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= () => _audioPlayer.PlaySound(ESound.PlayerDeath);
        if (_bonusDamageSkill != null) _bonusDamageSkill.onActivation -= (_, _) => _audioPlayer.PlaySound(ESound.RageEffect);
        if (_bonusRegenerationSkill != null) _bonusRegenerationSkill.onActivation -= (_, _) => _audioPlayer.PlaySound(ESound.HealthRegeneration);
        if (_throwGrenadeSkill != null) _throwGrenadeSkill.onActivation -= () => _audioPlayer.PlaySound(ESound.GrenadeThrow);
        if (_weaponSystem != null) _weaponSystem.onReload -= () => _audioPlayer.PlaySound(ESound.Reload);
        if (_weaponSystem != null) _weaponSystem.onShoot -= (shootSound) => _audioPlayer.PlaySound(shootSound);
    }
}
