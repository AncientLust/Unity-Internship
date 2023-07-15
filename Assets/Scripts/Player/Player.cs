using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        var rigidBody = gameObject.GetComponent<Rigidbody>();
        var experienceSystem = gameObject.AddComponent<PlayerExperienceSystem>();
        var inputSystem = gameObject.AddComponent<PlayerInputSystem>();
        var statsSystem = gameObject.AddComponent<PlayerStatsSystem>();
        var weaponSystem = gameObject.AddComponent<PlayerWeaponSystem>();
        var movementSystem = gameObject.AddComponent<PlayerMovementSystem>();
        var healthSystem = gameObject.AddComponent<PlayerHealthSystem>();
        var playerFacade = gameObject.AddComponent<PlayerSubsystems>();

        statsSystem.Init(experienceSystem);
        weaponSystem.Init(inputSystem, statsSystem);
        movementSystem.Init(statsSystem, inputSystem, rigidBody);
        healthSystem.Init(statsSystem);

        playerFacade.Init(
            experienceSystem,
            statsSystem,
            weaponSystem,
            healthSystem,
            inputSystem
        );
    }
}