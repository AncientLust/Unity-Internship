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
    public event Action onLoadPressed;
    public event Action onQuitPressed;

    private void OnEnable()
    {
        _menuButton.start.onClick.AddListener(() => onStartPressed.Invoke());
        _menuButton.start.onClick.AddListener(() => SetUI(EUI.HUD));
        _menuButton.load.onClick.AddListener(() => onLoadPressed.Invoke());
        _menuButton.quit.onClick.AddListener(() => onQuitPressed.Invoke());

        _menuButton.settings.onClick.AddListener(() => SetUI(EUI.Settings));
        _settingButton.back.onClick.AddListener(() => SetUI(EUI.Menu));
    }

    private void OnDisable()
    {
        _menuButton.start.onClick.RemoveListener(() => onStartPressed.Invoke());
        _menuButton.start.onClick.RemoveListener(() => SetUI(EUI.HUD));
        _menuButton.load.onClick.RemoveListener(() => onLoadPressed.Invoke());
        _menuButton.quit.onClick.RemoveListener(() => onQuitPressed.Invoke());

        _menuButton.settings.onClick.RemoveListener(() => SetUI(EUI.Settings));
        _settingButton.back.onClick.RemoveListener(() => SetUI(EUI.Menu));
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
