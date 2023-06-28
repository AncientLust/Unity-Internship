using UnityEngine;

public class Main : MonoBehaviour
{
    private SceneController _sceneController;

    private void Awake()
    {
        
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debug.Log("Congrats, now you have an entry point!");
        
        _sceneController = new SceneController();
        _sceneController.LoadMenu();
    }
}
