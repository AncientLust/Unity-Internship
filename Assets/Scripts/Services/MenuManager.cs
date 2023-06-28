using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private string _menu = "Menu";
    private string _settings = "Settings";
    private string _gameplay = "Game";

    public void StartGame()
    {
        SceneManager.LoadScene(_gameplay);
    }

    public void Menu()
    {
        MenuUI.Instance.SetScreen(_menu);
    }

    public void Settings()
    {
        MenuUI.Instance.SetScreen(_settings);
    }

    public void QuitGame()
    {
        Debug.Log("Application closed.");
        Application.Quit();
    }
}
