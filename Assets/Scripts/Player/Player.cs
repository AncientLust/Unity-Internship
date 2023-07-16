using UnityEngine;

public class Player : MonoBehaviour
{
    public void Init(ObjectPool objectPool)
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var experienceSystem = gameObject.AddComponent<PlayerExperienceSystem>();
        var inputSystem = gameObject.AddComponent<PlayerInputSystem>();
        var statsSystem = gameObject.AddComponent<PlayerStatsSystem>();
        var weaponSystem = gameObject.AddComponent<PlayerWeaponSystem>();
        var movementSystem = gameObject.AddComponent<PlayerMovementSystem>();
        var healthSystem = gameObject.AddComponent<PlayerHealthSystem>();
        var playerFacade = gameObject.AddComponent<PlayerFacade>();

        statsSystem.Init(experienceSystem);
        weaponSystem.Init(inputSystem, statsSystem, objectPool);
        movementSystem.Init(statsSystem, inputSystem, rigidBody);
        healthSystem.Init(statsSystem);

        playerFacade.Init(
            experienceSystem,
            statsSystem,
            weaponSystem,
            healthSystem,
            inputSystem,
            movementSystem
        );
    }
}