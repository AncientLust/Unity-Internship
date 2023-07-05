using UnityEngine;
using Enums;

public class Main : MonoBehaviour
{
    void Start()
    {        
        var objectFactory = new ObjectFactory();
        
        var eventSystem = objectFactory.InstantiateUndestroyable(EResource.EventSystem); // New gameobject + addComponent
        var cameraController = objectFactory.InstantiateUndestroyable(EResource.MainCamera).GetComponent<CameraController>(); 
        var uiRoot = objectFactory.InstantiateUndestroyable(EResource.UIRoot).GetComponent<UIRoot>();
        var hud = uiRoot.GetComponentInChildren<HUD>(true);

        var sceneSwitcher = new SceneSwitcher(uiRoot);
        var sceneLoader = new SceneObjectLoader(objectFactory, hud, sceneSwitcher, cameraController);
        var gameManager = new GameManager(uiRoot, sceneLoader, sceneSwitcher);
    }
}
