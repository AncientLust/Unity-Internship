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

    public void Init(UIRoot uiRoot, 
        SceneObjectBuilder sceneObjectLoader, 
        SceneController sceneController, 
        EnemySpawner enemySpawner,
        IPlayerFacade iPlayerFacade,
        ObjectPool objectPool,
        CameraController cameraController,
        PauseManager pauseManager)
    {
        _uiRoot = uiRoot;
        _sceneObjectLoader = sceneObjectLoader;
        _sceneController = sceneController;
        _enemySpawner = enemySpawner;
        _iPlayerFacade = iPlayerFacade;
        _objectPool = objectPool;
        _cameraController = cameraController;
        _pauseManager = pauseManager;

        Subscribe();
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
    }

    ~GameManager()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _sceneObjectLoader.onSceneObjectsBuild += (scene) => SceneLoadHandler(scene);
        _uiRoot.onStartPressed += PrepareGame;
        _uiRoot.onLoadPressed += LoadGame;
        _uiRoot.onQuitPressed += QuitGame;
        _uiRoot.onPausePressed += PauseGame;
        _uiRoot.onResumePressed += ResumeGame;
        _uiRoot.onPauseMenuPressed += OpenMenu;
        _uiRoot.onGameOverMenuPressed += OpenMenu;
        _uiRoot.onPauseRestartPressed += RestartGame;
        _uiRoot.onGameOverRestartPressed += RestartGame;
        _iPlayerFacade.onDie += GameOver;
    }

    private void Unsubscribe()
    {
        _sceneObjectLoader.onSceneObjectsBuild -= (scene) => StartGame();
        _uiRoot.onStartPressed -= PrepareGame;
        _uiRoot.onLoadPressed -= LoadGame;
        _uiRoot.onQuitPressed -= QuitGame;
        _uiRoot.onPausePressed -= PauseGame;
        _uiRoot.onResumePressed -= ResumeGame;
        _uiRoot.onPauseMenuPressed -= OpenMenu;
        _uiRoot.onGameOverMenuPressed -= OpenMenu;
        _uiRoot.onPauseRestartPressed -= RestartGame;
        _uiRoot.onGameOverRestartPressed -= RestartGame;
        _iPlayerFacade.onDie -= GameOver;
    }

    private void SceneLoadHandler(EScene scene)
    {
        switch (scene)
        {
            case EScene.GameSession:
                StartGame();
                return;
        }
    }

    private void PrepareGame()
    {
        _sceneController.LoadScene(EScene.Environment, LoadSceneMode.Additive);
        _sceneController.LoadScene(EScene.GameSession, LoadSceneMode.Additive);
    }

    private void StartGame()
    {
        _uiRoot.SetUI(EUI.HUD);
        _objectPool.Reset();
        _enemySpawner.StartSpawn();
        _iPlayerFacade.EnableForGameSession();
        _cameraController.MoveToPlayer();

        Debug.Log("Game started");
    }

    private void PauseGame()
    {
        _iPlayerFacade.SetInputHandling(false);
        _uiRoot.SetUI(EUI.Pause);
        _pauseManager.PauseGame();

        Debug.Log("Game paused");
    }

    private void ResumeGame()
    {
        _iPlayerFacade.SetInputHandling(true);
        _uiRoot.SetUI(EUI.HUD);
        _pauseManager.ResumeGame();
        
        Debug.Log("Game resumed");
    }

    private void OpenMenu()
    {
        _pauseManager.ResumeGame();
        _enemySpawner.StopSpawn();
        _uiRoot.SetUI(EUI.Menu);
        _sceneController.UnloadScene(EScene.Environment);
        _sceneController.UnloadScene(EScene.GameSession);
        
        Debug.Log("Menu opened");
    }

    private void LoadGame()
    {
        Debug.Log("Game loaded");
    }

    private void RestartGame()
    {
        _enemySpawner.StopSpawn();
        _sceneController.CleanScene(EScene.GameSession);
        _objectPool.Reset();
        _iPlayerFacade.EnableForGameSession();
        _enemySpawner.StartSpawn();
        _uiRoot.SetUI(EUI.HUD);
        _cameraController.MoveToPlayer();
        _pauseManager.ResumeGame();

        Debug.Log("Game restarted");
    }

    private void GameOver()
    {
        _iPlayerFacade.DisableForGameSession();
        _uiRoot.SetUI(EUI.GameOver);
        _pauseManager.PauseGame();

        Debug.Log("Game over");
    }

    private void QuitGame()
    {
        Debug.Log("Application closed");
        Application.Quit();
    }
}
