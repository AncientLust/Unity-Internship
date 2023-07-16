using UnityEngine.SceneManagement;
using Enums;
using UnityEngine;

public class SceneController
{
    public void LoadScene(EScene name, LoadSceneMode mode) // Add sanity check for the argument
    {
        SceneManager.LoadScene(name.ToString(), mode);
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
