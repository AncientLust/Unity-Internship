using System;
using UnityEngine;

public class EnemyExperienceSystem : MonoBehaviour
{
    private int _level = 1;
    private int _killExperience = 5;
    private float _levelsPerMinute = 3;
    private IExperienceTaker _experienceTaker;
    private EnemyHealthSystem _healthSystem;

    public Action<int> OnLevelChanged;

    public void Init
    (
        IExperienceTaker experienceTaker,
        EnemyHealthSystem healthSystem
    )
    {
        _experienceTaker = experienceTaker;
        _healthSystem = healthSystem;
        Subscribe();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie += TransferExperience;
    }

    private void Unsubscribe()
    {
        if (_healthSystem != null) _healthSystem.onDie -= TransferExperience;
    }

    public void ResetLevel()
    {
        SetLevelBasedOnGameDuration();
    }

    private void SetLevelBasedOnGameDuration() // Must be refactored
    {
        var minutesSceneLoaded = Time.timeSinceLevelLoad / 60.0f;
        _level = (int)Mathf.Ceil(minutesSceneLoaded * _levelsPerMinute);
        OnLevelChanged.Invoke(_level);
    }

    public void TransferExperience()
    {
        _experienceTaker.TakeExperience(_level * _killExperience);
    }
}
