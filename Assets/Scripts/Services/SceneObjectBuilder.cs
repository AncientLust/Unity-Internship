using System;
using UnityEngine;
using Enums;
using UnityEngine.SceneManagement;

public class SceneObjectBuilder
{
    private ObjectFactory _objectFactory;

    public Action<EScene> onSceneObjectsBuild;

    public void Init(ObjectFactory objectFactory)
    {
        _objectFactory = objectFactory;
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
        _objectFactory.Instantiate(EResource.Environment);
        _objectFactory.Instantiate(EResource.DirectionalLight);
    }
}
