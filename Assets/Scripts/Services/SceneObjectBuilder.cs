using System;
using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public class SceneObjectBuilder
{
    private GenericFactory _genericFactory;
    private GameSettings _gameSettings;   
    public Action<EScene> onSceneObjectsBuild;

    public void Init(GenericFactory genericFactory, GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        _genericFactory = genericFactory;
        SceneManager.sceneLoaded += SceneLoadHandler;
    }

    ~SceneObjectBuilder()
    {
        SceneManager.sceneLoaded -= SceneLoadHandler;
    }

    private void SceneLoadHandler(Scene scene, LoadSceneMode mode)
    {
        if (Enum.TryParse(scene.name, out EScene loadedScene))
        {
            SetSceneActive(loadedScene);
            LoadSceneObjects(loadedScene);
        }
        else
        {
            Debug.LogError($"Loaded {scene.name} scene wasn't found in EScene.");
        }
    }

    private void SetSceneActive(EScene scene)
    {
        var loadedScene = SceneManager.GetSceneByName(scene.ToString());
        SceneManager.SetActiveScene(loadedScene);
    }

    public void LoadSceneObjects(EScene scene)
    {
        switch (scene)
        {
            case EScene.LevelEnvironment:
                CreateLevelEnvironment();
                break;
            default:
                break;
        }

        onSceneObjectsBuild.Invoke(scene);
    }

    private void CreateLevelEnvironment()
    {
        switch (_gameSettings.LevelEnvironment)
        {
            case ELevelEnvironment.WarZone:
                _genericFactory.Instantiate(EResource.WarZone);
                break;
            case ELevelEnvironment.Forest:
                _genericFactory.Instantiate(EResource.Forest);
                break;
        }

        _genericFactory.Instantiate(EResource.DirectionalLight);
    }
}
