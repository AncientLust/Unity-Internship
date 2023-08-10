using System;
using UnityEngine;

public class EnemyExperienceSystem : MonoBehaviour
{
    private int _level = 1;
    private int _killExperience = 5;

    public Action<int> OnLevelChanged;

    public void SetLevel(int level)
    {
        _level = level;
        OnLevelChanged.Invoke(_level);
    }

    public float GetExperience()
    {
        return _level * _killExperience;
    }
}
