using System;
using UnityEngine;

public class EnemyExperienceSystem : MonoBehaviour, IExperienceMaker
{
    private int _level = 1;
    private int _killExperience = 10;
    private float _levelsPerMinute = 3;

    public Action<int> OnLevelChanged;

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

    public float MakeExperience()
    {
        return _level * _killExperience;
    }
}
