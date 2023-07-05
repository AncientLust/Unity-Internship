using System;
using UnityEngine;
using Enums;

public class SceneObjectLoader
{
    private ObjectFactory _objectFactory;
    private HUD _hud;
    private CameraController _cameraController;
    private SceneSwitcher _sceneSwitcher;
    
    private PlayerFacade _playerFacade;
    private EnemySpawner _enemySpawner;

    public Action<EScene> onSceneObjectsLoaded;

    public SceneObjectLoader(ObjectFactory objectFactory, HUD hud, SceneSwitcher sceneSwitcher, CameraController cameraController)
    {
        _objectFactory = objectFactory;
        _hud = hud;
        _sceneSwitcher = sceneSwitcher;
        _cameraController = cameraController;

        SubscribeEvents();
    }

    ~SceneObjectLoader()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _sceneSwitcher.onSceneLoaded += LoadSceneElements;
    }

    private void UnsubscribeEvents()
    {
        _sceneSwitcher.onSceneLoaded -= LoadSceneElements;
    }

    public void LoadSceneElements(EScene scene)
    {
        switch (scene)
        {
            case EScene.GameSession:
                CreateOtherObjects();
                CreatePlayer();
                InitServices();
                onSceneObjectsLoaded.Invoke(EScene.GameSession);
                break;
            default:
                break;
        }
    }

    private void CreatePlayer()
    {
        var player = _objectFactory.Instantiate(EResource.Player);

        var playerRigidBody = player.GetComponent<Rigidbody>();

        var playerExperienceSystem = player.AddComponent<PlayerExperienceSystem>();
        var playerInputSystem = player.AddComponent<PlayerInputSystem>();

        var playerStatsSystem = player.AddComponent<PlayerStatsSystem>();
        playerStatsSystem.Init(playerExperienceSystem);

        var playerWeaponSystem = player.AddComponent<PlayerWeaponSystem>();
        playerWeaponSystem.Init(playerInputSystem, playerStatsSystem);

        var playerMovementSystem = player.AddComponent<PlayerMovementSystem>();
        playerMovementSystem.Init(playerStatsSystem, playerInputSystem, playerRigidBody);

        var playerHealthSystem = player.AddComponent<PlayerHealthSystem>();
        playerHealthSystem.Init(playerStatsSystem);

        _playerFacade = player.AddComponent<PlayerFacade>();
        _playerFacade.Init(
            playerExperienceSystem,
            playerStatsSystem,
            playerWeaponSystem,
            playerHealthSystem
        );
    }

    private void CreateOtherObjects()
    {
        _objectFactory.Instantiate(EResource.Environment);
        _objectFactory.Instantiate(EResource.DirectionalLight);
        _enemySpawner = _objectFactory.Instantiate(EResource.EnemySpawner).GetComponent<EnemySpawner>();
    }

    private void InitServices()
    {
        _hud.Init(_playerFacade);
        _cameraController.Init(_playerFacade.transform);
        _enemySpawner.Init(_playerFacade.gameObject);
    }
}
