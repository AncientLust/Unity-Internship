using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject _player;
    
    private const string _gameplay = "Gameplay";
    private const string _pause = "Pause";
    private const string _gameOver = "GameOver";
    private const string _mainMenu = "MainMenu";

    public bool IsPaused { get; private set; }
    public bool IsStarted { get; private set; }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        IsStarted = true;
        EnemySpawner.Instance.enabled = true;
        GameplayUI.Instance.SetScreen(_gameplay);
    }

    public void PauseGame()
    {
        GameplayUI.Instance.SetScreen(_pause);
        IsPaused = true;
    }

    public void ResumeGame()
    {
        GameplayUI.Instance.SetScreen(_gameplay);
        IsPaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    public void GameOver()
    {
        GameplayUI.Instance.SetScreen(_gameOver);
        IsPaused = true;
    }
}
