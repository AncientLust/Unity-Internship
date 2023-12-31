using UnityEngine;

public class Player : MonoBehaviour
{
    public void Init(
        ObjectPool objectPool,
        IAudioPlayer audioPlayer,
        BonusRegenerationSkill bonusRegenerationSkill, 
        BonusDamageSkill bonusDamageSkill,
        ThrowGrenadeSkill throwGrenadeSkill,
        GameSettings gameSettings)
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var collider = gameObject.GetComponent<CapsuleCollider>();
        var experienceSystem = gameObject.AddComponent<PlayerExperienceSystem>();
        var inputSystem = gameObject.AddComponent<PlayerInputSystem>();
        var statsSystem = gameObject.AddComponent<PlayerStatsSystem>();
        var weaponSystem = gameObject.AddComponent<PlayerWeaponSystem>();
        var movementSystem = gameObject.AddComponent<PlayerMovementSystem>();
        var healthSystem = gameObject.AddComponent<PlayerHealthSystem>();
        var saveLoadSystem = gameObject.AddComponent<PlayerSaveLoadSystem>();
        var playerFacade = gameObject.AddComponent<PlayerFacade>();
        var skillSystem = gameObject.AddComponent<PlayerSkillSystem>();
        var effectsSystem = gameObject.AddComponent<PlayerEffectsSystem>();
        var throwSystem = gameObject.AddComponent<PlayerThrowSystem>();
        var animationSystem = gameObject.AddComponent<PlayerAnimationSystem>();

        var skills = new ISkill[] { bonusRegenerationSkill, bonusDamageSkill, throwGrenadeSkill };

        statsSystem.Init(experienceSystem, bonusRegenerationSkill, bonusDamageSkill);
        weaponSystem.Init(inputSystem, statsSystem, objectPool, healthSystem, audioPlayer);
        movementSystem.Init(statsSystem, inputSystem, rigidBody, healthSystem, collider);
        healthSystem.Init(statsSystem, experienceSystem, audioPlayer);
        saveLoadSystem.Init(experienceSystem, healthSystem);
        skillSystem.Init(inputSystem, skills);
        effectsSystem.Init(healthSystem, experienceSystem, bonusRegenerationSkill, bonusDamageSkill, gameSettings);
        throwSystem.Init(objectPool, throwGrenadeSkill);
        animationSystem.Init(healthSystem);
        inputSystem.Init(healthSystem);

        playerFacade.Init(
            experienceSystem,
            statsSystem,
            weaponSystem,
            healthSystem,
            inputSystem,
            movementSystem,
            saveLoadSystem,
            effectsSystem,
            skillSystem,
            animationSystem
        );
    }
}