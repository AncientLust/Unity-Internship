using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRoot : MonoBehaviour
{  
    [SerializeField] GameObject[] _screens;

    [SerializeField] private MenuButton _menuButton;
    [SerializeField] private SettingsButton _settingButton;
    [SerializeField] private PauseButton _pauseButton;
    [SerializeField] private GameOverButton _gameOverButton;

    public event Action onStartGame;
    public event Action onLoadGame;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        _menuButton.start.onClick.AddListener(() => onStartGame.Invoke());
        _menuButton.load.onClick.AddListener(() => onLoadGame.Invoke());
    }

    private void OnDisable()
    {
        _menuButton.start.onClick.RemoveListener(() => onStartGame.Invoke());
        _menuButton.load.onClick.RemoveListener(() => onLoadGame.Invoke());
    }

    public void SetUI(UIName name)
    {
        for (int i = 0; i < _screens.Length; i++)
        {
            _screens[i].gameObject.SetActive(_screens[i].name == name.ToString());
        }
    }

    [Serializable]
    private struct MenuButton
    {
        public Button start;
        public Button load;
        public Button settings;
        public Button quit;
    }

    [Serializable]
    private struct SettingsButton
    {
        public Button back;
    }

    [Serializable]
    private struct PauseButton
    {
        public Button resume;
        public Button save;
        public Button load;
        public Button restart;
        public Button menu;
    }

    [Serializable]
    private struct GameOverButton
    {
        public Button restart;
        public Button menu;
    }
}
