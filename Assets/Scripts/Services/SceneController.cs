using UnityEngine.SceneManagement;
using Enums;
using UnityEngine;

public class SceneController
{
    public void LoadScene(EScene name, LoadSceneMode mode)
    {
        string sceneName = name.ToString();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            if (System.IO.Path.GetFileNameWithoutExtension(scenePath) == sceneName)
            {
                SceneManager.LoadScene(sceneName, mode);
                return;
            }
        }

        Debug.LogError("Scene " + sceneName + " is not in the build settings.");
    }

    public void UnloadScene(EScene name)
    {
        SceneManager.UnloadSceneAsync(name.ToString());
    }

    public void CleanScene(EScene name)
    {
        var scene = SceneManager.GetSceneByName(name.ToString());
        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            GameObject.Destroy(obj); 
        }
    }
}
