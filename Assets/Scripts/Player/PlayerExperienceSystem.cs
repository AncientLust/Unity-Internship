using UnityEngine;

public class PlayerExperienceSystem : MonoBehaviour
{
    protected int _level = 1;
    protected ParticleSystem _levelUp;

    private float _experience = 0;
    private float _nextLevelExperienceStartValue = 50;
    private float _nextLevelExperience;
    private float _nextLevelMultiplier = 1.2f;

    public delegate void OnLevelChangedHandler(int level);
    public delegate void OnExperienceChangedHandler(float levelPercent);

    public event OnLevelChangedHandler OnLevelChanged;
    public event OnExperienceChangedHandler OnExperienceChanged;

    private void Awake()
    {
        CacheComponents();
    }

    private void Start()
    {
        _nextLevelExperience = _nextLevelExperienceStartValue * _nextLevelMultiplier;

        OnLevelChanged.Invoke(_level);
        OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
    }

    private void CacheComponents()
    {
        _levelUp = transform.Find("Effects/LevelUp").GetComponent<ParticleSystem>(); // Get rid of the string
    }

    public void AddExperience(float experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            LevelUp();
        }

        OnExperienceChanged.Invoke(_experience / _nextLevelExperience);
    }

    protected void LevelUp()
    {
        _level++;
        _experience = _experience - _nextLevelExperience;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        _levelUp.Play();
        OnLevelChanged.Invoke(_level);
    }

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _experience = 0;
        _level = level > 1 ? level : 1;
        _nextLevelExperience = _nextLevelExperienceStartValue * Mathf.Pow(_nextLevelMultiplier, _level);
        OnLevelChanged.Invoke(_level);
    }
}
