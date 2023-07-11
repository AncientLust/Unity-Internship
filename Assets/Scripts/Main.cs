using UnityEngine;
using Enums;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    private ObjectFactory _objectFactory;
    private StandaloneInputModule _eventSystem;
    private Transform _playerTransform;
    private Player _player;
    private PlayerFacade _playerFacade;
    private CameraController _cameraController;
    private UIRoot _uiRoot;
    private HUD _hud;
    private EnemySpawner _enemySpawner;
    private SceneLoader _sceneLoader;
    private SceneObjectBuilder _sceneObjectBuilder;
    private GameManager _gameManager;
    private IExperienceSystem _playerExperienceSystem;

    void Awake()
    {
        CreateObjects();
        LinkObjects();
    }

    private void CreateObjects()
    {
        _objectFactory = new ObjectFactory();
        _eventSystem = new GameObject("EventSystem").AddComponent<StandaloneInputModule>();
        _cameraController = _objectFactory.Instantiate(EResource.MainCamera).GetComponent<CameraController>();
        _uiRoot = _objectFactory.Instantiate(EResource.UIRoot).GetComponent<UIRoot>();
        _hud = _uiRoot.GetComponentInChildren<HUD>(true);
        _enemySpawner = _objectFactory.Instantiate(EResource.EnemySpawner).GetComponent<EnemySpawner>();
    
        _player = _objectFactory.Instantiate(EResource.Player).GetComponent<Player>();
        _playerFacade = _player.gameObject.GetComponent<PlayerFacade>();
        _playerTransform = _player.transform;
        _playerExperienceSystem = _playerFacade.ExperienceSystem;

        _sceneLoader = new SceneLoader();
        _sceneObjectBuilder = new SceneObjectBuilder();
        _gameManager = new GameManager();
    }

    private void LinkObjects()
    {
        _hud.Init(_playerFacade);
        _cameraController.Init(_playerTransform);
        _enemySpawner.Init(_playerTransform, _playerExperienceSystem);
        _sceneObjectBuilder.Init(_objectFactory);
        _gameManager.Init(_uiRoot, _sceneObjectBuilder, _sceneLoader, _enemySpawner, _playerFacade);
    }
}
