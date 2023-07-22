using System;
using UnityEngine;
using UnityEngine.UI;
using Enums;

public class UIRoot : MonoBehaviour
{  
    [SerializeField] GameObject[] _UIs;

    [SerializeField] private MenuButton _menuButton;
    [SerializeField] private HUDButton _hudButton;
    [SerializeField] private SettingsButton _settingButton;
    [SerializeField] private PauseButton _pauseButton;
    [SerializeField] private GameOverButton _gameOverButton;

    public event Action onStartPressed;
    public event Action onMenuLoadPressed;
    public event Action onPauseLoadPressed;
    public event Action onPauseSavePressed;
    public event Action onQuitPressed;
    public event Action onPausePressed;
    public event Action onResumePressed;
    public event Action onPauseRestartPressed;
    public event Action onPauseMenuPressed;
    public event Action onGameOverRestartPressed;
    public event Action onGameOverMenuPressed;

    private void Start()
    {
        SetUI(EUI.Menu);
    }

    private void OnEnable()
    {
        _menuButton.start.onClick.AddListener(() => onStartPressed.Invoke());
        _menuButton.load.onClick.AddListener(() => onMenuLoadPressed.Invoke());
        _menuButton.quit.onClick.AddListener(() => onQuitPressed.Invoke());

        _menuButton.settings.onClick.AddListener(() => SetUI(EUI.Settings));
        _settingButton.back.onClick.AddListener(() => SetUI(EUI.Menu));

        _hudButton.pause.onClick.AddListener(() => onPausePressed.Invoke());
        _pauseButton.resume.onClick.AddListener(() => onResumePressed.Invoke());
        _pauseButton.restart.onClick.AddListener(() => onPauseRestartPressed.Invoke());
        _pauseButton.menu.onClick.AddListener(() => onPauseMenuPressed.Invoke());
        _pauseButton.save.onClick.AddListener(() => onPauseSavePressed.Invoke());
        _pauseButton.load.onClick.AddListener(() => onPauseLoadPressed.Invoke());

        _gameOverButton.restart.onClick.AddListener(() => onGameOverRestartPressed.Invoke());
        _gameOverButton.menu.onClick.AddListener(() => onGameOverMenuPressed.Invoke());
    }

    private void OnDisable()
    {
        _menuButton.start.onClick.RemoveListener(() => onStartPressed.Invoke());
        _menuButton.load.onClick.RemoveListener(() => onMenuLoadPressed.Invoke());
        _menuButton.quit.onClick.RemoveListener(() => onQuitPressed.Invoke());

        _menuButton.settings.onClick.RemoveListener(() => SetUI(EUI.Settings));
        _settingButton.back.onClick.RemoveListener(() => SetUI(EUI.Menu));

        _hudButton.pause.onClick.RemoveListener(() => onPausePressed.Invoke());
        _pauseButton.resume.onClick.RemoveListener(() => onResumePressed.Invoke());
        _pauseButton.restart.onClick.RemoveListener(() => onPauseRestartPressed.Invoke());
        _pauseButton.menu.onClick.RemoveListener(() => onPauseMenuPressed.Invoke());
        _pauseButton.save.onClick.RemoveListener(() => onPauseSavePressed.Invoke());
        _pauseButton.load.onClick.RemoveListener(() => onPauseLoadPressed.Invoke());

        _gameOverButton.restart.onClick.RemoveListener(() => onGameOverRestartPressed.Invoke());
        _gameOverButton.menu.onClick.RemoveListener(() => onGameOverMenuPressed.Invoke());
    }

    public void SetUI(EUI name)
    {
        var nameString = name.ToString();
        var isNameFound = false;

        foreach (var ui in _UIs)
        {
            if (ui.name == nameString)
            {
                ui.SetActive(true);
                isNameFound = true;
            }
            else
            {
                ui.SetActive(false);
            }
        }

        if (!isNameFound)
        {
            Debug.LogError($"UI with name {nameString} was not found.");
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
    private struct HUDButton
    {
        public Button pause;
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
