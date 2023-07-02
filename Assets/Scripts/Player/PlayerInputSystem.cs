using System;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    public event Action onScrollUp;
    public event Action onScrollDown;
    public event Action onLeftMouseClicked;
    public event Action onReloadPressed;

    private void Update()
    {
        ReadInputIfGameRunning();
    }

    private void ReadInputIfGameRunning()
    {
        if (ShouldAct())
        {
            InputHandler();
        }
    }

    private bool ShouldAct()
    {
        return GameManager.Instance.IsStarted && !GameManager.Instance.IsPaused;
    }

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
