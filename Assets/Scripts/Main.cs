using UnityEngine;
using Enums;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class Main : MonoBehaviour
{
    private ObjectFactory _objectFactory;
    private ObjectPool _objectPool;
    private StandaloneInputModule _eventSystem;
    private Transform _playerTransform;
    private Player _player;
    private IPlayerFacade _iPlayerFacade;
    private IHUDCompatible _iHUDCompatible;
    private CameraController _cameraController;
    private UIRoot _uiRoot;
    private HUD _hud;
    private EnemySpawner _enemySpawner;
    private SceneController _sceneLoader;
    private SceneObjectBuilder _sceneObjectBuilder;
    private GameManager _gameManager;
    private IExperienceTaker _playerExperienceSystem;
    private PauseManager _pauseManager;

    void Awake()
    {
        CreateObjects();
        LinkObjects();
    }

    private void CreateObjects()
    {
        _objectFactory = new ObjectFactory();
        _objectPool = new ObjectPool();
        _pauseManager = new PauseManager();

        _eventSystem = new GameObject("EventSystem").AddComponent<StandaloneInputModule>();
        _cameraController = _objectFactory.Instantiate(EResource.MainCamera).GetComponent<CameraController>();
        _uiRoot = _objectFactory.Instantiate(EResource.UIRoot).GetComponent<UIRoot>();
        _hud = _uiRoot.GetComponentInChildren<HUD>(true);
        _enemySpawner = _objectFactory.Instantiate(EResource.EnemySpawner).GetComponent<EnemySpawner>();

        _player = _objectFactory.Instantiate(EResource.Player).GetComponent<Player>();
        _player.Init(_objectPool, 
            _uiRoot.GetComponentInChildren<BonusRegenerationSkill>(true),
            _uiRoot.GetComponentInChildren<BonusDamageSkill>(true)
            );
        
        _iHUDCompatible = _player.GetComponent<IHUDCompatible>();
        _iPlayerFacade = _player.GetComponent<IPlayerFacade>();

        _playerTransform = _player.GetComponent<ITargetable>().Transform;
        _playerExperienceSystem = _player.GetComponent<IExperienceTaker>();

        _sceneLoader = new SceneController();
        _sceneObjectBuilder = new SceneObjectBuilder();
        _gameManager = new GameManager();
    }

    private void LinkObjects()
    {
        _hud.Init(_iHUDCompatible);
        _sceneObjectBuilder.Init(_objectFactory);
        _cameraController.Init(_playerTransform);
        _enemySpawner.Init(_playerTransform, _playerExperienceSystem, _objectPool);
        _gameManager.Init(_uiRoot, _sceneObjectBuilder, _sceneLoader, _enemySpawner, _iPlayerFacade, _objectPool, _cameraController, _pauseManager);
    }
}
