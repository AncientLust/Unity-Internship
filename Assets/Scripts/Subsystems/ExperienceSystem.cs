using TMPro;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _levelPercent;
    [SerializeField] private ExperienceBar _experienceBar;
    [SerializeField] private ParticleSystem _levelUp;

    private float _experience = 0;
    private float _nextLevelExperience = 50;
    private float _nextLevelMultiplier = 1.2f;
    private StatsSystem _statSystem;

    public int Level { get; private set; } = 0;

    private void Start()
    {
        UpdateGUIExperienceElements();
        CacheComponents();
    }

    private void CacheComponents()
    {
        _statSystem = gameObject.GetComponent<StatsSystem>();
    }

    private void UpdateGUIExperienceElements()
    {
        _level.text = Level.ToString();
        _levelPercent.text = ((int)(_experience / _nextLevelExperience * 100)).ToString() + " %";
        _experienceBar.SetFill(_experience / _nextLevelExperience);
    }

    public void AddExperience(int experience)
    {
        _experience += experience;

        if (_experience >= _nextLevelExperience)
        {
            Level++;
            _experience = _experience - _nextLevelExperience;
            _nextLevelExperience *= _nextLevelMultiplier;
            _levelUp.Play();
            _statSystem.SetLevel(Level);
        }

        UpdateGUIExperienceElements();
    }
}
