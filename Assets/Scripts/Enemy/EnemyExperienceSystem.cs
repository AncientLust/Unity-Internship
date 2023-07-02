using System;
using UnityEngine;

public class EnemyExperienceSystem : MonoBehaviour
{
    protected int _level = 1;
    protected ParticleSystem _levelUp;

    public Action<int> OnLevelChanged;

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level > 1 ? level : 1;
        OnLevelChanged.Invoke(_level);
    }
}
