using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    public delegate void OnScrollUpHandler();
    public delegate void OnScrollDownHandler();
    public delegate void OnLeftMouseClickedHandler();
    public delegate void OnReloadPressedHandler();

    public event OnScrollUpHandler onScrollUp;
    public event OnScrollDownHandler onScrollDown;
    public event OnLeftMouseClickedHandler onLeftMouseClicked;
    public event OnReloadPressedHandler onReloadPressed;


    private void Update()
    {
        ReadInputIfGameRunning();
    }

    private void ReadInputIfGameRunning()
    {
        //if (ShouldAct())
        //{
        //    InputHandler();
        //}
    }

    //private bool ShouldAct()
    //{
    //    return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    //}

    private void InputHandler()
    {
        if (Input.GetMouseButton(0))
        {
            onLeftMouseClicked.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            onReloadPressed.Invoke();
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            onScrollDown.Invoke();
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            onScrollUp.Invoke();
        }
    }
}
