using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public class GameManager
{
    private UIRoot _uiRoot;
    private SceneController _sceneController;
    private SceneObjectBuilder _sceneObjectLoader;
    private EnemySpawner _enemySpawner;
    private ObjectPool _objectPool;
    private IPlayerFacade _iPlayerFacade;
    private CameraController _cameraController;
    private PauseManager _pauseManager;
    private LevelProgressManager _levelProgressManager;
    private IAudioPlayer _iAudioPlayer;
    private EGameSession _eGameSession;

    public void Init(UIRoot uiRoot, 
        SceneObjectBuilder sceneObjectLoader, 
        SceneController sceneController, 
        EnemySpawner enemySpawner,
        IPlayerFacade iPlayerFacade,
        ObjectPool objectPool,
        CameraController cameraController,
        PauseManager pauseManager,
        LevelProgressManager levelProgressManager,
        IAudioPlayer iAudioPlayer)
    {
        _uiRoot = uiRoot;
        _sceneObjectLoader = sceneObjectLoader;
        _sceneController = sceneController;
        _enemySpawner = enemySpawner;
        _iPlayerFacade = iPlayerFacade;
        _objectPool = objectPool;
        _cameraController = cameraController;
        _pauseManager = pauseManager;
        _levelProgressManager = levelProgressManager;
        _iAudioPlayer = iAudioPlayer;

        Subscribe();
    }

    ~GameManager()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _sceneObjectLoader.onSceneObjectsBuild += (scene) => SceneLoadHandler(scene);
        _uiRoot.onStartPressed += () => PrepareGame(EGameSession.New);
        _uiRoot.onMenuLoadPressed += () => PrepareGame(EGameSession.Loaded);
        _uiRoot.onPauseLoadPressed += LoadGameFromPause;
        _uiRoot.onQuitPressed += QuitGame;
        _uiRoot.onPausePressed += PauseGame;
        _uiRoot.onResumePressed += ResumeGame;
        _uiRoot.onPauseMenuPressed += OpenMenu;
        _uiRoot.onGameOverMenuPressed += OpenMenu;
        _uiRoot.onPauseRestartPressed += RestartGame;
        _uiRoot.onPauseSavePressed += SaveGame;
        _uiRoot.onGameOverRestartPressed += RestartGame;
        _iPlayerFacade.onDied += GameOver;
        _levelProgressManager.onLevelGoalReached += (_) => LevelCompleted();
        _uiRoot.onStartNextLevel += StartNextLevel;
    }

    private void Unsubscribe()
    {
        _sceneObjectLoader.onSceneObjectsBuild -= (scene) => SceneLoadHandler(scene);
        _uiRoot.onStartPressed -= () => PrepareGame(EGameSession.New);
        _uiRoot.onMenuLoadPressed -= () => PrepareGame(EGameSession.Loaded);
        _uiRoot.onPauseLoadPressed -= LoadGameFromPause;
        _uiRoot.onQuitPressed -= QuitGame;
        _uiRoot.onPausePressed -= PauseGame;
        _uiRoot.onResumePressed -= ResumeGame;
        _uiRoot.onPauseMenuPressed -= OpenMenu;
        _uiRoot.onGameOverMenuPressed -= OpenMenu;
        _uiRoot.onPauseRestartPressed -= RestartGame;
        _uiRoot.onPauseSavePressed -= SaveGame;
        _uiRoot.onGameOverRestartPressed -= RestartGame;
        _iPlayerFacade.onDied -= GameOver;
        _levelProgressManager.onLevelGoalReached -= (_) => LevelCompleted();
        _uiRoot.onStartNextLevel -= StartNextLevel;
    }

    private void PrepareGame(EGameSession eGameSession)
    {
        _sceneController.LoadScene(EScene.LevelEnvironment, LoadSceneMode.Additive);
        _sceneController.LoadScene(EScene.GameSession, LoadSceneMode.Additive);
        _eGameSession = eGameSession;
    }

    private void SceneLoadHandler(EScene scene)
    {
        switch (scene)
        {
            case EScene.GameSession:
                if (_eGameSession == EGameSession.New)
                {
                    StartGame();
                }

                if (_eGameSession == EGameSession.Loaded)
                {
                    LoadGameFromMenu();
                }
                return;
        }
    }

    private void StartGame()
    {
        _uiRoot.SetUI(EUI.HUD);
        _objectPool.Reset();
        _enemySpawner.StartSpawn();
        _enemySpawner.ResetEnemyLevel();
        _iPlayerFacade.EnableForGameSession();
        _cameraController.MoveToPlayer();
        _levelProgressManager.ResetProgress();
        _iAudioPlayer.PlayMusic(EMusic.Game);
    }

    private void PauseGame()
    {
        _iPlayerFacade.SetInputHandling(false);
        _uiRoot.SetUI(EUI.Pause);
        _pauseManager.PauseGame();
    }

    private void ResumeGame()
    {
        _iPlayerFacade.SetInputHandling(true);
        _uiRoot.SetUI(EUI.HUD);
        _pauseManager.ResumeGame();
    }

    private void OpenMenu()
    {
        _pauseManager.ResumeGame();
        _enemySpawner.StopSpawn();
        _uiRoot.SetUI(EUI.Menu);
        _sceneController.UnloadScene(EScene.LevelEnvironment);
        _sceneController.UnloadScene(EScene.GameSession);
        _iAudioPlayer.PlayMusic(EMusic.Lobby);
    }

    private void LoadGameFromMenu()
    {
        _uiRoot.SetUI(EUI.HUD);
        _objectPool.Reset();
        _enemySpawner.StartSpawn();
        _enemySpawner.ResetEnemyLevel();
        _iPlayerFacade.EnableForGameSession();
        _iPlayerFacade.LoadState();
        _cameraController.MoveToPlayer();
        _iAudioPlayer.PlayMusic(EMusic.Game);
    }

    private void LoadGameFromPause()
    {
        _enemySpawner.StopSpawn();
        _sceneController.CleanScene(EScene.GameSession);
        _objectPool.Reset();
        _iPlayerFacade.EnableForGameSession();
        _iPlayerFacade.LoadState();
        _enemySpawner.StartSpawn();
        _enemySpawner.ResetEnemyLevel();
        _uiRoot.SetUI(EUI.HUD);
        _cameraController.MoveToPlayer();
        _pauseManager.ResumeGame();
        _iAudioPlayer.PlayMusic(EMusic.Game);
    }

    private void SaveGame()
    {
        _iPlayerFacade.SaveState();
        Debug.Log("Game saved");
    }

    private void RestartGame()
    {
        _enemySpawner.StopSpawn();
        _sceneController.CleanScene(EScene.GameSession);
        _objectPool.Reset();
        _enemySpawner.StartSpawn();
        _enemySpawner.ResetEnemyLevel();
        _iPlayerFacade.EnableForGameSession();
        _uiRoot.SetUI(EUI.HUD);
        _levelProgressManager.ResetProgress();
        _cameraController.MoveToPlayer();
        _pauseManager.ResumeGame();
        _iAudioPlayer.PlayMusic(EMusic.Game);
    }

    private void GameOver()
    {
        _uiRoot.SetUI(EUI.GameOver);
        _pauseManager.PauseGame();
        _iAudioPlayer.FadeOutMusic();
    }

    private void LevelCompleted()
    {
        _iPlayerFacade.SetInputHandling(false);
        _pauseManager.PauseGame();
        _uiRoot.SetUI(EUI.LevelCompleted);
        _iAudioPlayer.PlaySound(ESound.LevelComplete);
        _iAudioPlayer.FadeOutMusic();
    }

    private void StartNextLevel()
    {
        _iPlayerFacade.SetInputHandling(true);
        _pauseManager.ResumeGame();
        _enemySpawner.StopSpawn();
        _sceneController.CleanScene(EScene.GameSession);
        _objectPool.Reset();
        _enemySpawner.StartSpawn();
        _iPlayerFacade.PrepareForNextLevel();
        _cameraController.MoveToPlayer();
        _uiRoot.SetUI(EUI.HUD);
        _iAudioPlayer.PlayMusic(EMusic.Game);
    }

    private void QuitGame()
    {
        Debug.Log("Application closed");
        Application.Quit();
    }
}
