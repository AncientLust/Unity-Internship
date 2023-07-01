using UnityEngine;

public class PlayerExperienceSystem : ExperienceSystem
{
    public delegate void OnLevelChangedHandler(int level);
    public delegate void OnExperienceChangedHandler(float levelPercent);

    public event OnLevelChangedHandler OnLevelChanged;
    public event OnExperienceChangedHandler OnExperienceChanged;

    private void Start()
    {
        _nextLevelExperience = _nextLevelExperienceStartValue * _nextLevelMultiplier;

        OnLevelChanged.Invoke(_level);
        OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
    }

    override public void AddExperience(float experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            LevelUp();
        }

        OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
    }

    override public int GetLevel()
    {
        return _level;
    }

    override public void SetLevel(int level)
    {
        _experience = 0;
        _level = level > 1 ? level : 1;
        _statSystem.SetLevelStats(_level);
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        OnLevelChanged.Invoke(_level);
    }
}
