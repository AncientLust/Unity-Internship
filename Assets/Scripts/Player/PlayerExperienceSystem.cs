using System;
using UnityEngine;

public class PlayerExperienceSystem : MonoBehaviour
{
    protected int _level = 1;

    private float _experience = 0;
    private float _nextLevelExperienceStartValue = 50;
    private float _nextLevelExperience;
    private float _nextLevelMultiplier = 1.2f;

    public event Action<int> onLevelChanged;
    public event Action<float> onExperienceProgressChanged;

    private void Start()
    {
        _nextLevelExperience = _nextLevelExperienceStartValue * _nextLevelMultiplier;
    }

    protected void LevelUp()
    {
        _level++;
        _experience = _experience - _nextLevelExperience;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        onLevelChanged.Invoke(_level);
    }

    public void AddExperience(float experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            LevelUp();
        }

        onExperienceProgressChanged.Invoke(_experience / _nextLevelExperience);
    }

    public void SetLevel(int level)
    {
        _experience = 0;
        _level = level > 1 ? level : 1;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        onLevelChanged.Invoke(_level);
        onExperienceProgressChanged.Invoke(0);
    }

    public int GetLevel()
    {
        return _level;
    }

    public float GetExperience() 
    {
        return _experience;
    }
}
