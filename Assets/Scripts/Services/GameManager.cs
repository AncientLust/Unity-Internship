using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _player;
    
    private static bool _startGameImmediately = false;
    private const string _gameplay = "Gameplay";
    private const string _pause = "Pause";
    private const string _gameOver = "GameOver";

    public bool IsPaused { get; private set; }
    public bool IsStarted { get; private set; }


    private void Start()
    {
        StartGameImmediately();
    }

    private void StartGameImmediately()
    {
        if (_startGameImmediately)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        IsStarted = true;
        EnemySpawner.Instance.enabled = true;
        UIManager.Instance.SetScreen(_gameplay);
        _player.SetActive(true);
    }

    public void PauseGame()
    {
        UIManager.Instance.SetScreen(_pause);
        IsPaused = true;
    }

    public void ResumeGame()
    {
        UIManager.Instance.SetScreen(_gameplay);
        IsPaused = false;
    }

    public void RestartGame()
    {
        _startGameImmediately = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        _startGameImmediately = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        UIManager.Instance.SetScreen(_gameOver);
        IsPaused = true;
    }
}
