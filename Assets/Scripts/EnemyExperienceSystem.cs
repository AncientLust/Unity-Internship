using UnityEngine;

public class EnemyExperienceSystem : ExperienceSystem
{
    private void Start()
    {
        _nextLevelExperience = _nextLevelExperienceStartValue * _nextLevelMultiplier;
    }

    override public void AddExperience(float experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            LevelUp();
        }
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
    }
}
