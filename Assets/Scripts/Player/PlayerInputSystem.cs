using System;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    public event Action onScrollUp;
    public event Action onScrollDown;
    public event Action onLeftMouseClicked;
    public event Action onReloadPressed;
    public event Action<Vector3> onDirectionAxisPressed;

    private Vector3 _directionVetor;

    private void Update()
    {
        ReadInputIfGameRunning();
    }

    private void ReadInputIfGameRunning()
    {
        InputHandler();
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

        _directionVetor.x = Input.GetAxisRaw("Horizontal");
        _directionVetor.z = Input.GetAxisRaw("Vertical");

        if (_directionVetor.x != 0 || _directionVetor.z != 0)
        {
            onDirectionAxisPressed.Invoke(_directionVetor);
        }
    }
}
