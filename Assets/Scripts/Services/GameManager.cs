using System;
using UnityEngine;

public class GameManager
{
    private UIRoot _uiRoot;
    private SceneSwitcher _sceneController;
    private SceneObjectLoader _sceneObjectLoader;

    //public Action onGameStarted;
    //public bool IsPaused { get; private set; }
    //public bool IsStarted { get; private set; }

    public GameManager(UIRoot uiRoot, SceneObjectLoader sceneObjectLoader, SceneSwitcher sceneController)
    {
        _uiRoot = uiRoot;
        _sceneObjectLoader = sceneObjectLoader;
        _sceneController = sceneController;

        SubscribeEvents();
    }

    ~GameManager()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _sceneObjectLoader.onSceneObjectsLoaded += (scene) => StartGame();
        _uiRoot.onLoadPressed += LoadGame;
        _uiRoot.onQuitPressed += QuitGame;
    }

    private void UnsubscribeEvents()
    {
        _sceneObjectLoader.onSceneObjectsLoaded -= (scene) => StartGame();
        _uiRoot.onLoadPressed -= LoadGame;
        _uiRoot.onQuitPressed -= QuitGame;
    }

    private void StartGame()
    {
        //_sceneController.LoadScene(Enums.Scene.Game);
        //_sceneLoader.LoadGameElements();
        Debug.Log("Game started");
        //IsStarted = true;
        //EnemySpawner.Instance.enabled = true;
        //GameplayUI.Instance.SetScreen(_gameplay);
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
