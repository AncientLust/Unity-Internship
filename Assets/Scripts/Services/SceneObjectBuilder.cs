using System;
using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public class SceneObjectBuilder
{
    private GenericFactory _genericFactory;

    public Action<EScene> onSceneObjectsBuild;

    public void Init(GenericFactory genericFactory)
    {
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
            case EScene.Environment:
                CreateLevelEnvironment();
                break;
            default:
                break;
        }

        onSceneObjectsBuild.Invoke(scene);
    }

    private void CreateLevelEnvironment()
    {
        //_genericFactory.Instantiate(EResource.Environment);
        //_genericFactory.Instantiate(EResource.DirectionalLight);
    }
}
