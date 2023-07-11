using UnityEngine.SceneManagement;
using Enums;

public class SceneLoader
{
    public void LoadScene(EScene name, LoadSceneMode mode) // Add sanity check for the argument
    {
        SceneManager.LoadScene(name.ToString(), mode);
    }
}
