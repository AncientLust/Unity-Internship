using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public class GameManager
{
    private UIRoot _uiRoot;
    private SceneLoader _sceneLoader;
    private SceneObjectBuilder _sceneObjectLoader;
    private EnemySpawner _enemySpawner;
    private PlayerFacade _playerFacade;

    public void Init(UIRoot uiRoot, 
        SceneObjectBuilder sceneObjectLoader, 
        SceneLoader sceneLoader, 
        EnemySpawner enemySpawner, 
        PlayerFacade playerFacade)
    {
        _uiRoot = uiRoot;
        _sceneObjectLoader = sceneObjectLoader;
        _sceneLoader = sceneLoader;
        _enemySpawner = enemySpawner;
        _playerFacade = playerFacade;

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
    }

    private void Unsubscribe()
    {
        _sceneObjectLoader.onSceneObjectsBuild -= (scene) => StartGame();
        _uiRoot.onStartPressed -= PrepareGame;
        _uiRoot.onLoadPressed -= LoadGame;
        _uiRoot.onQuitPressed -= QuitGame;
    }

    private void SceneLoadHandler(EScene scene)
    {
        switch (scene)
        {
            case EScene.GameSession:
                StartGame();
                break;
        }
    }

    private void PrepareGame()
    {
        _sceneLoader.LoadScene(EScene.Environment, LoadSceneMode.Additive);
        _sceneLoader.LoadScene(EScene.GameSession, LoadSceneMode.Additive);
    }

    private void StartGame()
    {
        _uiRoot.SetUI(EUI.HUD);
        _enemySpawner.StartSpawn();
        _playerFacade.InputSystem.IsActive = true;
        Debug.Log("Game started");
    }

    private void PauseGame()
    {
        Debug.Log("Game paused");
        //GameplayUI.Instance.SetScreen(_pause);
        //IsPaused = true;
    }

    private void LoadGame()
    {
        Debug.Log("Game loaded");
    }

    private void ResumeGame()
    {
        Debug.Log("Game resumed");
        //GameplayUI.Instance.SetScreen(_gameplay);
        //IsPaused = false;
    }

    private void RestartGame()
    {
        Debug.Log("Game restarted");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        Debug.Log("Game over");
        //GameplayUI.Instance.SetScreen(_gameOver);
        //IsPaused = true;
    }

    private void QuitGame()
    {
        Debug.Log("Application closed");
        Application.Quit();
    }
}
