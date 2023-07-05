using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using Enums;

public class SceneSwitcher 
{
    private UIRoot _uiRoot;
    public Action<EScene> onSceneLoaded;

    public SceneSwitcher(UIRoot uiRoot)
    {
        _uiRoot = uiRoot;
        SubscribeEvents();
    }

    ~SceneSwitcher()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _uiRoot.onStartPressed += () => LoadScene(EScene.GameSession);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void UnsubscribeEvents()
    {
        _uiRoot.onStartPressed -= () => LoadScene(EScene.GameSession);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public void LoadScene(EScene name)
    {
        SceneManager.LoadScene(name.ToString());
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (Enum.TryParse(scene.name, out EScene loadedScene))
        {
            onSceneLoaded.Invoke(loadedScene);
        }
        else
        {
            Debug.LogError($"Loaded scene with name {scene.name} was not found in Enum.Scene!");
        }
    }
}
