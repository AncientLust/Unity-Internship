using UnityEngine;

public class Player : MonoBehaviour
{
    public void Init(
        ObjectPool objectPool, 
        BonusRegenerationSkill bonusRegenerationSkill, 
        BonusDamageSkill bonusDamageSkill,
        ThrowGrenadeSkill throwGrenadeSkill)
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
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

        var skills = new ISkill[] { bonusRegenerationSkill, bonusDamageSkill, throwGrenadeSkill };

        statsSystem.Init(experienceSystem, bonusRegenerationSkill, bonusDamageSkill);
        weaponSystem.Init(inputSystem, statsSystem, objectPool);
        movementSystem.Init(statsSystem, inputSystem, rigidBody);
        healthSystem.Init(statsSystem, experienceSystem);
        saveLoadSystem.Init(experienceSystem, healthSystem);
        skillSystem.Init(inputSystem, skills);
        effectsSystem.Init(healthSystem, experienceSystem, bonusRegenerationSkill, bonusDamageSkill);
        throwSystem.Init(objectPool, throwGrenadeSkill);

        playerFacade.Init(
            experienceSystem,
            statsSystem,
            weaponSystem,
            healthSystem,
            inputSystem,
            movementSystem,
            saveLoadSystem,
            effectsSystem,
            skillSystem
        );
    }
}