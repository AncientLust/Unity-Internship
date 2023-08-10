using Enums;
using System;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{
    private bool _isActive;
    private Vector3 _directionVetor;
    private PlayerHealthSystem _healthSystem;
    private bool _isInitialized;

    public event Action onScrollUp;
    public event Action onScrollDown;
    public event Action onLeftMouseClicked;
    public event Action onReloadPressed;
    public event Action<Vector3> onDirectionAxisPressed;
    public event Action<ESkillSlot> onSkill1Clicked;

    public void Init(PlayerHealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _isActive = false;
        _isInitialized = true;
        Subscribe();
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            Subscribe();
        }
    }

    private void OnDisable()
    {
        Unsubsribe();
    }

    private void Subscribe()
    {
        _healthSystem.onDie += () => _isActive = false;
    }

    private void Unsubsribe()
    {
        _healthSystem.onDie -= () => _isActive = false;
    }

    private void Update()
    {
        ReadInputIfGameRunning();
    }

    private void ReadInputIfGameRunning()
    {
        if (_isActive)
        {
            InputHandler();
        }
    }

    private void InputHandler()
    {
        HandleMouseInput();
        HandleAxisInput();
        HandleAxisInput();
        HandleSkillsInput();
    }

    private void HandleMouseInput()
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

    private void HandleAxisInput()
    {
        _directionVetor.x = Input.GetAxisRaw("Horizontal");
        _directionVetor.z = Input.GetAxisRaw("Vertical");

        if (_directionVetor.x != 0 || _directionVetor.z != 0)
        {
            onDirectionAxisPressed.Invoke(_directionVetor);
        }
    }

    private void HandleSkillsInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            onSkill1Clicked.Invoke(ESkillSlot._1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            onSkill1Clicked.Invoke(ESkillSlot._2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            onSkill1Clicked.Invoke(ESkillSlot._3);
        }
    }

    public void SetActive(bool state)
    {
        _isActive = state;
    }
}
