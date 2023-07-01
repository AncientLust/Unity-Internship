using UnityEngine;

public abstract class ExperienceSystem : MonoBehaviour
{

    protected int _level = 1;
    protected float _experience = 0;
    protected float _nextLevelExperienceStartValue = 50;
    protected float _nextLevelExperience;
    protected float _nextLevelMultiplier = 1.2f;
    protected StatsSystem _statSystem;
    protected Player _player;
    protected ParticleSystem _levelUp;

    abstract public int GetLevel();
    abstract public void SetLevel(int level);
    abstract public void AddExperience(float experience);

    private void Awake()
    {
        CacheComponents();
    }

    private void CacheComponents()
    {
        _statSystem = gameObject.GetComponent<StatsSystem>();
        _levelUp = transform.Find("Effects/LevelUp").GetComponent<ParticleSystem>();
    }

    protected void LevelUp()
    {
        _level++;
        _experience = _experience - _nextLevelExperience;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        _levelUp.Play();
        _statSystem.SetLevelStats(_level);
    }
}
