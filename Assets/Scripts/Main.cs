using UnityEngine;
using Enums;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    private GenericFactory _genericFactory;
    private ProjectileFactory _projectileFactory;
    private EnemyFactory _enemyFactory;
    private ObjectPool _objectPool;
    private StandaloneInputModule _eventSystem;
    private AudioPlayer _audioPlayer;
    private IAudioPlayer _iAudioPlayer;
    private Transform _playerTransform;
    private Player _player;
    private IPlayerFacade _iPlayerFacade;
    private IHUDCompatible _iHUDCompatible;
    private CameraController _cameraController;
    private UIRoot _uiRoot;
    private GameSettings _gameSettings;
    private HUD _hud;
    private EnemySpawner _enemySpawner;
    private SceneController _sceneLoader;
    private SceneObjectBuilder _sceneObjectBuilder;
    private GameManager _gameManager;
    private LevelProgressManager _levelProgressManager;
    private IExperienceTaker _iExperienceTaker;
    private PauseManager _pauseManager;
    private EnemyDisposalManager _enemyDisposalManager;
    private LevelCompletedUI _levelCompletedUI;
    private BonusRegenerationSkill _bonusRegenerationSkill;
    private BonusDamageSkill _bonusDamageSkill;
    private ThrowGrenadeSkill _throwGrenadeSkill;

    void Awake()
    {
        CreateObjects();
        InitObjects();
    }

    private void CreateObjects()
    {
        _genericFactory = new GenericFactory();
        _projectileFactory = new ProjectileFactory();
        _enemyFactory = new EnemyFactory();
        _objectPool = new ObjectPool();
        
        _pauseManager = new PauseManager();
        _sceneLoader = new SceneController();
        _sceneObjectBuilder = new SceneObjectBuilder();
        _gameManager = new GameManager();
        _levelProgressManager = new LevelProgressManager();
        _enemyDisposalManager = new EnemyDisposalManager();

        _eventSystem = new GameObject("EventSystem").AddComponent<StandaloneInputModule>();
        _audioPlayer = new GameObject("AudioPlayer").AddComponent<AudioPlayer>();
        _iAudioPlayer = _audioPlayer.GetComponent<IAudioPlayer>();
        
        _cameraController = _genericFactory.Instantiate(EResource.MainCamera).GetComponent<CameraController>();
        _uiRoot = _genericFactory.Instantiate(EResource.UIRoot).GetComponent<UIRoot>();
        _hud = _uiRoot.GetComponentInChildren<HUD>(true);
        _gameSettings = _uiRoot.GetComponentInChildren<GameSettings>(true);
        _levelCompletedUI = _uiRoot.GetComponentInChildren<LevelCompletedUI>(true);
        _enemySpawner = _genericFactory.Instantiate(EResource.EnemySpawner).GetComponent<EnemySpawner>();

        _bonusRegenerationSkill = _uiRoot.GetComponentInChildren<BonusRegenerationSkill>(true);
        _bonusDamageSkill = _uiRoot.GetComponentInChildren<BonusDamageSkill>(true);
        _throwGrenadeSkill = _uiRoot.GetComponentInChildren<ThrowGrenadeSkill>(true);

        _player = _genericFactory.Instantiate(EResource.Player).GetComponent<Player>();
        _player.Init(
            _objectPool,
            _iAudioPlayer,
            _bonusRegenerationSkill,
            _bonusDamageSkill,
            _throwGrenadeSkill,
            _gameSettings
        );

        _iHUDCompatible = _player.GetComponent<IHUDCompatible>();
        _iPlayerFacade = _player.GetComponent<IPlayerFacade>();

        _playerTransform = _player.transform;
        _iExperienceTaker = _player.GetComponent<IExperienceTaker>();
    }

    private void InitObjects()
    {
        _hud.Init(_iHUDCompatible, _levelProgressManager);
        _sceneObjectBuilder.Init(_genericFactory);
        _cameraController.Init(_playerTransform);
        _enemySpawner.Init(_playerTransform, _objectPool, _enemyDisposalManager, _levelProgressManager);
        _enemyDisposalManager.Init(_iExperienceTaker, _objectPool);
        _levelCompletedUI.Init(_levelProgressManager);
        _levelProgressManager.Init(_enemyDisposalManager);
        _audioPlayer.Init(_gameSettings);

        _objectPool.Init(_projectileFactory, _enemyFactory);
        _projectileFactory.Init(_objectPool);
        _enemyFactory.Init(_audioPlayer, _gameSettings, _objectPool, _playerTransform);

        _bonusRegenerationSkill.Init(_iAudioPlayer);
        _bonusDamageSkill.Init(_iAudioPlayer);
        _throwGrenadeSkill.Init(_iAudioPlayer);

        _gameManager.Init(
            _uiRoot, 
            _sceneObjectBuilder, 
            _sceneLoader, 
            _enemySpawner, 
            _iPlayerFacade, 
            _objectPool, 
            _cameraController, 
            _pauseManager,
            _levelProgressManager,
            _audioPlayer
        );
    }
}
