using UnityEngine.SceneManagement;

public class SceneController
{
    private const string _menu = "Menu";
    private const string _game = "Game";

    public void LoadMenu()
    {
        LoadScene(_menu);
    }

    public void LoadGame()
    {
        LoadScene(_game);
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
